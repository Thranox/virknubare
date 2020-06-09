using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Specifications;
using Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.ApplicationServices
{
    public class UserCustomerStatusServiceIntegrationTests
    {
        [Test]
        public async Task PutAsync_ValidInput_UpdatesUserRelation()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = testContext.ServiceProvider.GetService<IUserCustomerStatusService>();

                var userEntity = testContext.GetUnitOfWork().Repository
                    .List(new UserBySub(TestData.DummyInitialSubEdward))
                    .Single();

                // Act
                var actual = await sut.PutAsync(testContext.GetPolApiContext(TestData.DummyAdminSubDennis), userEntity
                    .Id, testContext.GetDummyCustomer1Id(), (int)UserStatus.UserAdministrator);

                // Assert
                var userAfterChange = testContext.GetUnitOfWork().Repository.List(new UserBySub(userEntity.Subject)).Single();
                var customerUserPermissionEntityAfterChange = userAfterChange.CustomerUserPermissions.Single(x => x.CustomerId == testContext.GetDummyCustomer1Id());
                Assert.That(customerUserPermissionEntityAfterChange.UserStatus == UserStatus.UserAdministrator);
            }
        }

        [Test]
        public async Task CreateCustomerStatusAsync_ValidInput_UpdatesUserRelation()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = testContext.ServiceProvider.GetService<IUserCustomerStatusService>();

                var userEntity = testContext.GetUnitOfWork().Repository
                    .List(new UserBySub(TestData.DummyPolSubAlice))
                    .Single();

                // Act
                var actual = await sut.CreateCustomerStatusAsync(testContext.GetPolApiContext(TestData.DummyPolSubAlice),new []{ testContext.GetDummyCustomer2Id() } );

                // Assert
                var userAfterChange = testContext.GetUnitOfWork().Repository.List(new UserBySub(userEntity.Subject)).Single();
                var customerUserPermissionEntityAfterChange = userAfterChange.CustomerUserPermissions.Single(x => x.CustomerId == testContext.GetDummyCustomer2Id());
                Assert.That(customerUserPermissionEntityAfterChange.UserStatus == UserStatus.Initial);
            }
        }
    }
}