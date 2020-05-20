using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Shared.Controllers;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.API.Controllers
{
    public class UserCustomerStatusControllerIntegrationTests
    {
        private static Mock<ISubManagementService> _subManagementService;

        [SetUp]
        public void SetUp()
        {
            _subManagementService = new Mock<ISubManagementService>();
        }

        [Test]
        public async Task Put_ExistingUser_IsSuccess()
        {
            // Arrange
            // Set executing user to be Dennis -- he is admin
            _subManagementService.Setup(x => x.GetSub(It.IsAny<ClaimsPrincipal>()))
                .Returns(TestData.DummyAdminSubDennis);
            using (var testContext = new IntegrationTestContext())
            {
                var userEntityEdward = testContext
                    .CreateUnitOfWork()
                    .Repository
                    .List(new UserBySub(TestData.DummyInitialSubEdward))
                    .SingleOrDefault();

                var sut = GetSut(testContext);

                // Act
                var actual = await sut.Put(userEntityEdward.Id, testContext.GetDummyCustomerId(), "Registered");

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as UserCustomerStatusPutResponse;
                Assert.That(value, Is.Not.Null);
            }
        }

        private static UserCustomerStatusController GetSut(IntegrationTestContext testContext)
        {
            return new UserCustomerStatusController(_subManagementService.Object,
                testContext.ServiceProvider.GetService<IUserCustomerStatusService>());
        }
    }
}