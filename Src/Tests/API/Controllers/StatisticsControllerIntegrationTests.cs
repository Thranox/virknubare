using System.Threading.Tasks;
using API.Shared.Controllers;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.API.Controllers
{
    public class StatisticsControllerIntegrationTests
    {
        private static Mock<ISubManagementService> _subManagementService;

        [SetUp]
        public void SetUp()
        {
            _subManagementService = new Mock<ISubManagementService>();
        }

        [Test]
        [Explicit("Awaits 2020-46")]
        public async Task Get_NoParameters_ReturnsTravelExpenses()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                _subManagementService.Setup(x => x.GetPolApiContext(It.IsAny<HttpContext>()))
                    .ReturnsAsync(testContext.GetPolApiContext(TestData.DummyPolSubAlice));

                var sut = GetSut(testContext);

                // Act
                var actual = await sut.Post();

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as StatisticsGetResponse;
                Assert.That(value, Is.Not.Null);
            }
        }

        private static StatisticsController GetSut(IntegrationTestContext testContext)
        {
            return new StatisticsController(_subManagementService.Object,
                testContext.ServiceProvider.GetService<IGetStatisticsService>());
        }
    }
}