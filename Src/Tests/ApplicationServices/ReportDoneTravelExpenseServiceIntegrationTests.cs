using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Specifications;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Tests.TestHelpers;

namespace Tests.ApplicationServices
{
    public class ReportDoneTravelExpenseServiceIntegrationTests
    {
        [Test]
        public async Task ReportDoneAsync_ExistingTravelExpense_MarksTravelExpenseAsReportedDone()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                Guid existingId;
                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var existing = unitOfWork.Repository.List<TravelExpenseEntity>().First();
                    existingId = existing.Id;
                    var travelExpenseUpdateDto = new TravelExpenseReportDoneDto {Id = existingId};
                    var sut = testContext.ServiceProvider.GetService<IReportDoneTravelExpenseService>();

                    // Act
                    var actual = await sut.ReportDoneAsync(travelExpenseUpdateDto);

                    // Assert
                    Assert.That(actual, Is.Not.Null);
                }

                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var travelExpenseEntity = unitOfWork
                        .Repository.List(new TravelExpenseById(existingId))
                        .Single();
                    Assert.That(travelExpenseEntity.IsReportedDone, Is.EqualTo(true));
                    Assert.That(travelExpenseEntity.IsCertified, Is.EqualTo(false));
                    Assert.That(travelExpenseEntity.IsAssignedPayment, Is.EqualTo(false));
                }
            }
        }
    }
}