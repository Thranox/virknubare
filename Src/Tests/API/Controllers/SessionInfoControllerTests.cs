using System.Threading.Tasks;
using API.Shared.Controllers;
using API.Shared.Services;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.API.Controllers
{
    public class SessionInfoControllerTests
    {
        private static Mock<ISubManagementService> _subManagementService;

        [SetUp]
        public void SetUp()
        {
            _subManagementService = new Mock<ISubManagementService>();
        }

        [Test]
        public async Task Get_NoParameters_ReturnsInfoForUser()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                _subManagementService.Setup(x => x.GetPolApiContext(It.IsAny<HttpContext>()))
                    .ReturnsAsync(testContext.GetPolApiContext(TestData.DummyPolSubAlice));

                var sut = GetSut(testContext);

                // Act
                var actual = await sut.Get();

            }
        }

        private UserInfoController GetSut(IntegrationTestContext testContext)
        {
            return new UserInfoController(
                _subManagementService.Object,
                testContext.ServiceProvider.GetService<IGetUserInfoService>());
        }
    }

}