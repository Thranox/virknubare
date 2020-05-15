using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Specifications;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.ApplicationServices
{
    public class UserCustomerStatusServiceIntegrationTests
    {
        [Test]
        [Ignore("Not done")]
        public async Task GetAsync_NoParameters_ReturnsFlowSteps()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = testContext.ServiceProvider.GetService<IUserCustomerStatusService>();

                var userEntityAdmin = testContext.CreateUnitOfWork().Repository
                    .List(new UserBySub(TestData.DummyPolSubAlice))
                    .Single();

                // Act
                var actual = await sut.PutAsync(TestData.DummyAdminSubDennis, userEntityAdmin
                    .Id,testContext.GetDummyCustomerId(),"admin");

                //// Assert
                //Assert.That(actual, Is.Not.Null);
                Assert.Fail();
            }
        }
    }
}