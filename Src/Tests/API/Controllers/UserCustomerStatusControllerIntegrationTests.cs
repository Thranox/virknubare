using System.Linq;
using System.Threading.Tasks;
using API.Shared.Controllers;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using Domain.Specifications;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using TestHelpers;
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
            using var testContext = new IntegrationTestContext();

            // Set executing user to be Dennis -- he is admin
            _subManagementService.Setup(x => x.GetPolApiContext(It.IsAny<HttpContext>()))
                .ReturnsAsync(testContext.GetPolApiContext(TestData.DummyAdminSubDennis));

            var userEntityEdward = testContext
                .GetUnitOfWork()
                .Repository
                .List(new UserBySub(TestData.DummyInitialSubEdward))
                .SingleOrDefault();

            var sut = GetSut(testContext);

            // Act
            var actual = await sut.Put(userEntityEdward.Id, testContext.GetDummyCustomer1Id(),
                (int) UserStatus.Registered);

            // Assert
            Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
            var okObjectResult = actual.Result as OkObjectResult;
            Assert.That(okObjectResult, Is.Not.Null);
            var value = okObjectResult.Value as UserCustomerStatusPutResponse;
            Assert.That(value, Is.Not.Null);
        }

        [Test]
        public async Task Post_ExistingCustomer_IsSuccess()
        {
            // Arrange
            using var testContext = new IntegrationTestContext();
            
            // Set executing user to be alice -- she has only association with dummy customer
            var subOfUser = TestData.DummyPolSubAlice;
            _subManagementService.Setup(x => x.GetPolApiContext(It.IsAny<HttpContext>()))
                .ReturnsAsync(testContext.GetPolApiContext(subOfUser));

            var sut = GetSut(testContext);

            // Act
            var userCustomerPostDto = new UserCustomerPostDto {CustomerIds = new[] {testContext.GetDummyCustomer2Id()}};
            var actual = await sut.Post(userCustomerPostDto);

            // Assert
            Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
            var okObjectResult = actual.Result as OkObjectResult;
            Assert.That(okObjectResult, Is.Not.Null);
            var value = okObjectResult.Value as UserCustomerPostResponse;
            Assert.That(value, Is.Not.Null);
            Assert.That(value.Ids.Count, Is.EqualTo(userCustomerPostDto.CustomerIds.Count() + 1));
        }

        private static UserCustomerStatusController GetSut(IntegrationTestContext testContext)
        {
            return new UserCustomerStatusController(_subManagementService.Object,
                testContext.ServiceProvider.GetService<IUserCustomerStatusService>());
        }
    }
}