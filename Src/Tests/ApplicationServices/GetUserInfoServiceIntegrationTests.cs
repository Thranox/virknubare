using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Specifications;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.ApplicationServices
{
    public class GetUserInfoServiceIntegrationTests
    {
        [Test]
        public async Task GetAsync_Alice_ReturnsUserInfoForAlice()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = testContext.ServiceProvider.GetService<IGetUserInfoService>();

                // Act
                var actual = await sut.GetAsync(TestData.DummyPolSubAlice);

                // Assert
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual.UserCustomerInfo.Length, Is.EqualTo(1));
                var userCustomerInfo = actual.UserCustomerInfo.Single();
                var customerEntity = testContext
                    .CreateUnitOfWork()
                    .Repository
                    .List(
                        new CustomerByName(TestData.DummyCustomerName)
                    )
                    .SingleOrDefault();
                Assert.That(userCustomerInfo.CustomerId, Is.EqualTo(customerEntity.Id));
                Assert.That(userCustomerInfo.CustomerName, Is.EqualTo(customerEntity.Name));
            }
        }
   }
}