using System;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Controllers;
using Application.Dtos;
using AutoFixture;
using Domain.Interfaces;
using Domain.Specifications;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.Flows
{
    public class FlowTests
    {
        [Test]
        public async Task EndToEndUsingControllers()
        {
            using (var testContext = new IntegrationTestContext())
            {
                // Arrange
                var customerId = testContext.GetDummyCustomerId();
                var travelExpenseController = testContext.ServiceProvider.GetRequiredService<TravelExpenseController>();
                var flowStepController = testContext.ServiceProvider.GetRequiredService<FlowStepController>();

                // Set the calling user to be Alice
                testContext.SetCallingUserBySub(TestData.DummyPolSubAlice);

                // ** Create TravelExpense
                var actionResultTePost = await travelExpenseController.Post(
                    new TravelExpenseCreateDto
                        {Description = testContext.Fixture.Create<string>(), CustomerId = customerId});

                var travelExpenseId =
                    ((actionResultTePost.Result as CreatedResult).Value as TravelExpenseCreateResponse).Id;

                Console.WriteLine("-----");

                Console.WriteLine("After posting:");
                Console.WriteLine(JsonConvert.SerializeObject(
                    ((await travelExpenseController.GetById(travelExpenseId)).Result as OkObjectResult).Value as
                    TravelExpenseGetByIdResponse, Formatting.Indented));

                // ** Report TravelExpense Done
                var actionResulltFsGet = await flowStepController.Get();
                var okObjectResult = actionResulltFsGet.Result as OkObjectResult;
                var flowStepGetResponse = okObjectResult.Value as FlowStepGetResponse;

                Assert.That(flowStepGetResponse.Result.Any());
                var firstFlowStep = flowStepGetResponse.Result.First();
                await travelExpenseController.Process(travelExpenseId, firstFlowStep.FlowStepId);

                Console.WriteLine("After Reporting done:");
                Console.WriteLine(JsonConvert.SerializeObject(
                    ((await travelExpenseController.GetById(travelExpenseId)).Result as OkObjectResult).Value as
                    TravelExpenseGetByIdResponse, Formatting.Indented));


                // ** Certify TravelExpense


                // Set the calling user to be Bob
                testContext.SetCallingUserBySub(TestData.DummySekSubBob);

                actionResulltFsGet = await flowStepController.Get();
                okObjectResult = actionResulltFsGet.Result as OkObjectResult;
                flowStepGetResponse = okObjectResult.Value as FlowStepGetResponse;

                Assert.That(flowStepGetResponse.Result.Any());

                firstFlowStep = flowStepGetResponse.Result.First();
                await travelExpenseController.Process(travelExpenseId, firstFlowStep.FlowStepId);

                Console.WriteLine("After certifying");

                // Set the calling user to be Alice
                testContext.SetCallingUserBySub(TestData.DummyPolSubAlice);

                Console.WriteLine(JsonConvert.SerializeObject(
                    ((await travelExpenseController.GetById(travelExpenseId)).Result as OkObjectResult).Value as
                    TravelExpenseGetByIdResponse, Formatting.Indented));


                // ** Assign TravelExpense for payment


                // Set the calling user to be Charlie
                testContext.SetCallingUserBySub(TestData.DummyLedSubCharlie);

                actionResulltFsGet = await flowStepController.Get();
                okObjectResult = actionResulltFsGet.Result as OkObjectResult;
                flowStepGetResponse = okObjectResult.Value as FlowStepGetResponse;

                Assert.That(flowStepGetResponse.Result.Any());

                firstFlowStep = flowStepGetResponse.Result.First();
                await travelExpenseController.Process(travelExpenseId, firstFlowStep.FlowStepId);

                Console.WriteLine("After Assigning Payment");

                // Set the calling user to be Alice
                testContext.SetCallingUserBySub(TestData.DummyPolSubAlice);
                Console.WriteLine(JsonConvert.SerializeObject(
                    ((await travelExpenseController.GetById(travelExpenseId)).Result as OkObjectResult).Value as
                    TravelExpenseGetByIdResponse, Formatting.Indented));
            }
        }

        [Test]
        public void TestInitialToFinal()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var processFlowSteps = testContext
                    .ServiceProvider
                    .GetServices<IProcessFlowStep>()
                    .ToArray();

                var customer = testContext
                    .GetUnitOfWork()
                    .Repository
                    .List(new CustomerByName(TestData.DummyCustomerName1))
                    .Single();

                var newTe = customer.TravelExpenses.First();

                var iterations = 0;

                // Act & Assert
                do
                {
                    iterations++;
                    Console.WriteLine("Stage: " + newTe.Stage);

                    var nextFlowSteps = testContext
                        .GetUnitOfWork()
                        .Repository
                        .List(new FlowStepByCustomer(newTe.Stage.Value, customer.Id))
                        .First();

                    var processFlowStep = processFlowSteps
                        .SingleOrDefault(x => x.CanHandle(nextFlowSteps.Key));

                    Assert.That(processFlowStep, Is.Not.Null);

                    newTe.ApplyProcessStep(processFlowStep);

                    // Guard -- should this test not come to a natural ending, it should fail, not continue forever.
                    if (iterations > 10)
                        throw new InvalidOperationException();
                } while (newTe.Stage.Value != TravelExpenseStage.Final);
            }
        }
    }
}