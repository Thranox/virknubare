using System.Linq;
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
    public class CustomerControllerIntegrationTests
    {
        private static Mock<ISubManagementService> _subManagementService;

        [SetUp]
        public void SetUp()
        {
            _subManagementService = new Mock<ISubManagementService>();
        }

        [Test]
        public async Task GetCustomerUsers_ExistingCustomer_ReturnsUsersWithStatus()
        {
            // Arrange
            //_subManagementService.Setup(x => x.GetPolApiContext(It.IsAny<HttpContext>()))
            //    .Returns(TestData.DummyPolSubAlice);
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

        [Test]
        public async Task PostInvitations_ExistingCustomer_ReturnsUsersWithStatus()
        {
            // Arrange
            var customerInvitationsPostDto = new CustomerInvitationsPostDto { Emails = new[] { "user1@domain.com", "user2@domain.com" } };
            using (var testContext = new IntegrationTestContext())
            {
                _subManagementService.Setup(x => x.GetPolApiContext(It.IsAny<HttpContext>()))
                    .ReturnsAsync(testContext.GetPolApiContext(TestData.DummyPolSubAlice));
                var sut = GetSut(testContext);

                // Act
                var actual = await sut.PostInvitations(testContext.GetDummyCustomerId(), customerInvitationsPostDto);

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as CustomerInvitationsPostResponse;
                Assert.That(value, Is.Not.Null);
            }
        }

        private static CustomerController GetSut(IntegrationTestContext testContext)
        {
            return new CustomerController(_subManagementService.Object,
                testContext.ServiceProvider.GetService<ICustomerUserService>());
        }
    }
}