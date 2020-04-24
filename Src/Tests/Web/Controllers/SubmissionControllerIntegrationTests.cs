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
using SharedWouldBeNugets;
using Tests.TestHelpers;
using Web.Controllers;
using Web.Services;

namespace Tests.Web.Controllers
{
    public class SubmissionControllerIntegrationTests
    {
        [Test]
        [Explicit("Awaits 2020-26")]
        public async Task Get_NoParameters_ReturnsTravelExpenses()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = GetSut(testContext);

                // Act
                var actual = await sut.Post();

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as SubmissionPostResponse;
                Assert.That(value, Is.Not.Null);
            }
        }

        private static SubmissionController GetSut(IntegrationTestContext testContext)
        {
            var subManagementService = new Mock<ISubManagementService>();
            subManagementService.Setup(x => x.GetSub(It.IsAny<ClaimsPrincipal>())).Returns(TestData.DummyPolSubAlice);

            return new SubmissionController(subManagementService.Object,
                testContext.ServiceProvider.GetService<ICreateSubmissionService>());
        }
    }
}