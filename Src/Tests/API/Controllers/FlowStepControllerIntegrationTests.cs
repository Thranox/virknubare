using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Shared.Controllers;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.API.Controllers
{
    public class FlowStepControllerIntegrationTests
    {
        [Test]
        public async Task Get_NoParameters_ReturnsTravelExpenses()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = GetSut(testContext);

                // Act
                var actual = await sut.Get();

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as FlowStepGetResponse;
                Assert.That(value, Is.Not.Null);
                var resultArray = value.Result.ToArray();
                Assert.That(resultArray.Length, Is.EqualTo(4));
                var customer = testContext
                    .CreateUnitOfWork()
                    .Repository
                    .List(new CustomerByName(TestData.DummyCustomerName))
                    .Single();

                var stageEntityInitial = testContext
                    .CreateUnitOfWork()
                    .Repository
                    .List(new StageByValue(TravelExpenseStage.Initial))
                    .Single();

                var stageEntityReportedDone = testContext
                    .CreateUnitOfWork()
                    .Repository
                    .List(new StageByValue(TravelExpenseStage.ReportedDone))
                    .Single();

                var stageEntityCertified = testContext
                    .CreateUnitOfWork()
                    .Repository
                    .List(new StageByValue(TravelExpenseStage.Certified))
                    .Single();

                var stageEntityAssignedForPayment = testContext
                    .CreateUnitOfWork()
                    .Repository
                    .List(new StageByValue(TravelExpenseStage.AssignedForPayment))
                    .Single();

                var stageEntityFinal = testContext
                    .CreateUnitOfWork()
                    .Repository
                    .List(new StageByValue(TravelExpenseStage.Final))
                    .Single();

                Assert.That(resultArray,
                    Has.One.EqualTo(new FlowStepDto()
                    {
                        CustomerName = customer.Name,
                        FromStageId = stageEntityInitial.Id,
                        FromStageText = Globals.StageNamesDanish[TravelExpenseStage.Initial],
                        Key = Globals.InitialReporteddone,
                        CustomerId = customer.Id
                    }));
                Assert.That(resultArray,
                    Has.One.EqualTo(new FlowStepDto()
                    {
                        CustomerName = customer.Name,
                        FromStageId = stageEntityReportedDone.Id,
                        FromStageText = Globals.StageNamesDanish[TravelExpenseStage.ReportedDone],
                        Key = Globals.ReporteddoneCertified,
                        CustomerId = customer.Id,
                    }));
                Assert.That(resultArray,
                    Has.One.EqualTo(new FlowStepDto()
                    {
                        CustomerName = customer.Name,
                        FromStageId = stageEntityCertified.Id,
                        FromStageText = Globals.StageNamesDanish[TravelExpenseStage.Certified],
                        Key = Globals.CertifiedAssignedForPayment,
                        CustomerId = customer.Id,
                    }));
                Assert.That(resultArray,
                    Has.One.EqualTo(new FlowStepDto()
                    {
                        CustomerName = customer.Name,
                        FromStageId = stageEntityAssignedForPayment.Id,
                        FromStageText = Globals.StageNamesDanish[TravelExpenseStage.AssignedForPayment],
                        Key = Globals.AssignedForPaymentFinal,
                        CustomerId = customer.Id,
                    }));
            }
        }

        private static FlowStepController GetSut(IntegrationTestContext testContext)
        {
            var subManagementService = new Mock<ISubManagementService>();
            subManagementService.Setup(x => x.GetSub(It.IsAny<ClaimsPrincipal>())).Returns(TestData.DummyPolSubAlice);

            return new FlowStepController(subManagementService.Object,
                testContext.ServiceProvider.GetService<IGetFlowStepService>());
        }
    }
}