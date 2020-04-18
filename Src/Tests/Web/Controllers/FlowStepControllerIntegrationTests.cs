using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Tests.TestHelpers;
using Web.Controllers;
using Web.Services;

namespace Tests.Web.Controllers
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
                var v = value.Result.ToArray();
                Assert.That(v.Length, Is.EqualTo(4));
                Assert.That(v,
                    Has.One.EqualTo(new FlowStepDto()
                    {
                        From = TravelExpenseStage.Initial.ToString(),
                        Key = Globals.InitialReporteddone
                    }));
                Assert.That(v,
                    Has.One.EqualTo(new FlowStepDto()
                    {
                        From = TravelExpenseStage.ReportedDone.ToString(),
                        Key = Globals.ReporteddoneCertified
                    }));
                Assert.That(v,
                    Has.One.EqualTo(new FlowStepDto()
                    {
                        From = TravelExpenseStage.Certified.ToString(),
                        Key = Globals.CertifiedAssignedForPayment
                    }));
                Assert.That(v,
                    Has.One.EqualTo(new FlowStepDto()
                    {
                        From = TravelExpenseStage.AssignedForPayment.ToString(),
                        Key = Globals.AssignedForPaymentFinal
                    }));
            }
        }

        private static FlowStepController GetSut(IntegrationTestContext testContext)
        {
            var subManagementService = new Mock<ISubManagementService>();
            subManagementService.Setup(x => x.GetSub(It.IsAny<ClaimsPrincipal>())).Returns(TestData.DummyPolSub);

            return new FlowStepController(subManagementService.Object,
                testContext.ServiceProvider.GetService<IGetFlowStepService>());
        }
    }
}