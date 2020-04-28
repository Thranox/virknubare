using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API.Shared.Controllers;
using API.Shared.Services;
using Application.Dtos;
using AutoFixture;
using Domain;
using Domain.Interfaces;
using Domain.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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
                ((FakeSubManagementService) testContext.ServiceProvider.GetRequiredService<ISubManagementService>())
                    .Sub= TestData.DummyPolSubAlice;


                var actionResultTePost = await travelExpenseController.Post(
                    new TravelExpenseCreateDto()
                        {Description = testContext.Fixture.Create<string>(), CustomerId = customerId});

                var travelExpenseId = ((actionResultTePost.Result as CreatedResult).Value as TravelExpenseCreateResponse).Id;

                // Set the calling user to be Bob
                ((FakeSubManagementService)testContext.ServiceProvider.GetRequiredService<ISubManagementService>())
                    .Sub = TestData.DummySekSubBob;
                
                
                var actionResulltFsGet = await flowStepController.Get();
                var okObjectResult = actionResulltFsGet.Result as OkObjectResult;
                var flowStepGetResponse = okObjectResult.Value as FlowStepGetResponse;

                Assert.That(flowStepGetResponse.Result.Any());

                //TODO Assert.Fail();
            }
        }

        [Test]
        public void TestInitialToFinal()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var buildServiceProvider = new ServiceCollection();

                Assembly
                    .GetAssembly(typeof(IProcessFlowStep))
                    .GetTypesAssignableFrom<IProcessFlowStep>()
                    .ForEach(t => { buildServiceProvider.AddScoped(typeof(IProcessFlowStep), t); });

                var serviceProvider = buildServiceProvider.BuildServiceProvider();
                var processFlowSteps = serviceProvider
                    .GetServices<IProcessFlowStep>();

                using (var polDbContext = new PolDbContext(testContext.DbContextOptions))
                {
                    var repository = new EfRepository(polDbContext);
                    var customer = repository
                        .List(new CustomerByName(TestData.DummyCustomerName))
                        .Single();

                    var newTe = customer.TravelExpenses.First();

                    var iterations = 0;

                    // Act & Assert
                    do
                    {
                        iterations++;
                        Console.WriteLine("Stage: " + newTe.Stage);

                        var nextFlowSteps = testContext.CreateUnitOfWork().Repository
                            .List(new FlowStepByCustomer(newTe.Stage, customer.Id))
                            .First();

                        var processFlowStep = processFlowSteps
                            .SingleOrDefault(x => x.CanHandle(nextFlowSteps.Key));

                        Assert.That(processFlowStep, Is.Not.Null);

                        newTe.ApplyProcessStep(processFlowStep);

                        if (iterations > 10)
                            throw new InvalidOperationException();
                    } while (newTe.Stage != TravelExpenseStage.Final);
                }
            }
        }
    }
}