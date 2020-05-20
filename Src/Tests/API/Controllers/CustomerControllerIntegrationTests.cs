using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Shared.Controllers;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.API.Controllers
{
    public class CustomerControllerIntegrationTests
    {
        [Test]
        public async Task GetCustomerUsers_ExistingCustomer_ReturnsUsersWithStatus()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = GetSut(testContext);

                // Act
                var actual = await sut.GetCustomerUsers(testContext.GetDummyCustomerId());

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as CustomerUserGetResponse;
                Assert.That(value.Users.Length, Is.EqualTo(TestData.GetTestUsers().Count()));
            }
        }

        private static CustomerController GetSut(IntegrationTestContext testContext)
        {
            var subManagementService = new Mock<ISubManagementService>();
            subManagementService.Setup(x => x.GetSub(It.IsAny<ClaimsPrincipal>())).Returns(TestData.DummyPolSubAlice);

            return new CustomerController(subManagementService.Object,
                testContext.ServiceProvider.GetService<ICustomerUserService>());
        }
    }
}