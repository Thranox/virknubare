//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Application.Dtos;
//using Application.Interfaces;
//using Domain.Entities;
//using Domain.Specifications;
//using Microsoft.Extensions.DependencyInjection;
//using NUnit.Framework;
//using Tests.TestHelpers;

//namespace Tests.ApplicationServices
//{
//    public class CertifyTravelExpenseServiceIntegrationTests
//    {
//        [Test]
//        public async Task CertifyAsync_ExistingTravelExpense_MarksTravelExpenseAsCertified()
//        {
//            // Arrange
//            using (var testContext = new IntegrationTestContext())
//            {
//                Guid existingId;
//                using (var unitOfWork = testContext.CreateUnitOfWork())
//                {
//                    var existing = unitOfWork.Repository.List<TravelExpenseEntity>().First();
//                    //existing.ReportDone();
//                    existingId = existing.Id;
//                    unitOfWork.Commit();
//                }
//                var travelExpenseCertifyDto = new TravelExpenseCertifyDto { Id = existingId };
//                var sut = testContext.ServiceProvider.GetService<ICertifyTravelExpenseService>();

//                // Act
//                var actual = await sut.CertifyAsync(travelExpenseCertifyDto);

//                // Assert
//                Assert.That(actual, Is.Not.Null);

//                using (var unitOfWork = testContext.CreateUnitOfWork())
//                {
//                    var travelExpenseEntity = unitOfWork
//                        .Repository.List(new TravelExpenseById(existingId))
//                        .Single();
//                    Assert.That(travelExpenseEntity.IsReportedDone, Is.EqualTo(true));
//                    Assert.That(travelExpenseEntity.IsCertified, Is.EqualTo(true));
//                    Assert.That(travelExpenseEntity.IsAssignedPayment, Is.EqualTo(false));
//                }
//            }
//        }
//    }
//}