using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using AutoFixture;
using Domain;
using Domain.Entities;
using Domain.Specifications;
using Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.ApplicationServices
{
    public class UpdateTravelExpenseServiceIntegrationTests
    {
        [Test]
        public async Task UpdateAsync_ExistingTravelExpense_UpdatesTravelExpenses()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                Guid existingId;
                var newDescription = testContext.Fixture.Create<string>();
                using (var unitOfWork = testContext.GetUnitOfWork())
                {
                    var existing = unitOfWork.Repository.List<TravelExpenseEntity>().First();
                    existingId = existing.Id;
                    var travelExpenseUpdateDto = new TravelExpenseUpdateDto
                        {Description = newDescription};
                    var sut = testContext.ServiceProvider.GetService<IUpdateTravelExpenseService>();

                    // Act
                    var actual = await sut.UpdateAsync(testContext.GetPolApiContext(TestData.DummyPolSubAlice), existingId, travelExpenseUpdateDto);

                    // Assert
                    Assert.That(actual, Is.Not.Null);
                }

                using (var unitOfWork = testContext.GetUnitOfWork())
                {
                    var travelExpenseEntity = unitOfWork
                        .Repository.List(new TravelExpenseById(existingId))
                        .SingleOrDefault();
                    Assert.That(travelExpenseEntity, Is.Not.Null);
                    Assert.That(travelExpenseEntity.Stage.Value, Is.EqualTo(TravelExpenseStage.Initial));
                    Assert.That(travelExpenseEntity.Description, Is.EqualTo(newDescription));
                }
            }
        }
    }
}