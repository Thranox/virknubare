using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.ApplicationServices
{
    public class CustomerUserServiceIntegrationTests
    {
        [Test]
        public async Task GetAsync_AdminUserCallingWithExistingCustomer_ReturnsUsersWithStuff()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = testContext.ServiceProvider.GetService<ICustomerUserService>();

                // Act
                var actual = await sut.GetAsync(TestData.DummyPolSubAlice, testContext.GetDummyCustomerId());

                // Assert
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual.Users.Length, Is.EqualTo(TestData.GetTestUsers().Count()));
            }
        }
    }
}