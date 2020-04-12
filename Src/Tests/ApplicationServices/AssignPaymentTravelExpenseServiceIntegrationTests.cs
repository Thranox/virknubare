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
    public class AssignPaymentTravelExpenseServiceIntegrationTests
    {
        [Test]
        public async Task CertifyAsync_ExistingTravelExpense_MarksTravelExpenseAsCertified()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                Guid existingId;
                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var existing = unitOfWork.Repository.List<TravelExpenseEntity>().First();
                    existing.ReportDone();
                    existing.Certify();
                    existingId = existing.Id;
                    unitOfWork.Commit();
                }
                var travelExpenseAssignPaymentDto = new TravelExpenseAssignPaymentDto { Id = existingId };
                var sut = testContext.ServiceProvider.GetService<IAssignPaymentTravelExpenseService>();

                // Act
                var actual = await sut.AssignPaymentAsync(travelExpenseAssignPaymentDto);

                // Assert
                Assert.That(actual, Is.Not.Null);

                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var travelExpenseEntity = unitOfWork
                        .Repository.List(new TravelExpenseById(existingId))
                        .Single();
                    Assert.That(travelExpenseEntity.IsReportedDone, Is.EqualTo(true));
                    Assert.That(travelExpenseEntity.IsCertified, Is.EqualTo(true));
                    Assert.That(travelExpenseEntity.IsAssignedPayment, Is.EqualTo(true));
                }
            }
        }
    }
}