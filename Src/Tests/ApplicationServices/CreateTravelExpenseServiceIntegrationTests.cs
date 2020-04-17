using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using AutoFixture;
using Domain;
using Domain.Specifications;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
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
                var travelExpenseCreateDto = new TravelExpenseCreateDto { Description = newDescription };
                var sut = testContext.ServiceProvider.GetService<ICreateTravelExpenseService>();

                // Act
                var actual = await sut.CreateAsync(travelExpenseCreateDto, Globals.DummyPolSub);

                // Assert
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual.Id, Is.Not.EqualTo(Guid.Empty));

                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var travelExpenseEntity = unitOfWork
                        .Repository.List(new TravelExpenseById(actual.Id))
                        .SingleOrDefault();
                    Assert.That(travelExpenseEntity, Is.Not.Null);
                    Assert.That(travelExpenseEntity.IsReportedDone, Is.EqualTo(false));
                    Assert.That(travelExpenseEntity.IsCertified, Is.EqualTo(false));
                }

            }
        }
    }
}