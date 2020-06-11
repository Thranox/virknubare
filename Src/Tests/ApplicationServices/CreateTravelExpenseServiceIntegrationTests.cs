using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using AutoFixture;
using Domain;
using Domain.Specifications;
using Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.ApplicationServices
{
    public class CreateTravelExpenseServiceIntegrationTests
    {
        [Test]
        public async Task CreateAsync_ValidInput_CreatesTravelExpense()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var newDescription = testContext.Fixture.Create<string>();
                var customerId = testContext.GetDummyCustomer1Id();
                var travelExpenseCreateDto = new TravelExpenseCreateDto { Description = newDescription, CustomerId = customerId};
                var sut = testContext.ServiceProvider.GetService<ICreateTravelExpenseService>();

                // Act
                var actual = await sut.CreateAsync(testContext.GetPolApiContext(TestData.DummyPolSubAlice), travelExpenseCreateDto);

                // Assert
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual.Id, Is.Not.EqualTo(Guid.Empty));

                using (var unitOfWork = testContext.GetUnitOfWork())
                {
                    var travelExpenseEntity = unitOfWork
                        .Repository.List(new TravelExpenseById(actual.Id))
                        .SingleOrDefault();
                    Assert.That(travelExpenseEntity, Is.Not.Null);
                    Assert.That(travelExpenseEntity.Stage.Value, Is.EqualTo(TravelExpenseStage.Initial));
                }

            }
        }
    }
}