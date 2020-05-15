using System.Security.Claims;
using System.Threading.Tasks;
using API.Shared.Controllers;
using API.Shared.Services;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.API.Controllers
{
    public class SessionInfoControllerTests
    {
        [Test]
        public async Task Get_NoParameters_ReturnsInfoForUser()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = GetSut(testContext);

                // Act
                var actual = await sut.Get();

            }
        }

        private UserInfoController GetSut(IntegrationTestContext testContext)
        {
            var subManagementService = new Mock<ISubManagementService>();
            subManagementService.Setup(x => x.GetSub(It.IsAny<ClaimsPrincipal>()))
                .Returns(TestData.DummyPolSubAlice);

            return new UserInfoController(subManagementService.Object,
                testContext.ServiceProvider.GetService<IGetUserInfoService>());
        }
    }

}