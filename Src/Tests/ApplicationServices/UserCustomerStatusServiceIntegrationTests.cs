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
                    .Id,testContext.GetDummyCustomerId(),(int)UserStatus.UserAdministrator);

                // Assert
                var userAfterChange = testContext.GetUnitOfWork().Repository.List(new UserBySub(userEntity.Subject)).Single();
                var customerUserPermissionEntityAfterChange = userAfterChange.CustomerUserPermissions.Single(x=>x.CustomerId==testContext.GetDummyCustomerId());
                Assert.That(customerUserPermissionEntityAfterChange.UserStatus==UserStatus.UserAdministrator);
            }
        }
    }
}