using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using Domain.Exceptions;
using Domain.Specifications;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.ApplicationServices
{
    public class ProcessStepTravelExpenseServiceIntegrationTests
    {
        [Test]
        public void ProcessStepAsync_InvalidUser_Throws()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = testContext.ServiceProvider.GetService<IProcessStepTravelExpenseService>();

                // Act & Assert
                var itemNotFoundException =
                    Assert.ThrowsAsync<ItemNotFoundException>(() => sut.ProcessStepAsync(new TravelExpenseProcessStepDto(),  Guid.Empty.ToString()));
                Assert.That(itemNotFoundException.Id, Is.EqualTo(Guid.Empty.ToString()));
                Assert.That(itemNotFoundException.Item, Is.EqualTo("UserEntity"));
            }
        }

        [Test]
        public async Task ProcessStepAsync_ValidTravelExpenseAndStep_Returns()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var travelExpenseProcessStepDto = new TravelExpenseProcessStepDto()
                {
                    ProcessStepKey = Globals.InitialReporteddone,
                    TravelExpenseId = testContext.TravelExpenseEntity1.Id
                };
                var sut = testContext.ServiceProvider.GetService<IProcessStepTravelExpenseService>();

                // Act
                var travelExpenseProcessStepResponse =await sut.ProcessStepAsync(travelExpenseProcessStepDto,  TestData.DummyPolSubAlice);

                // Assert
                Assert.That(travelExpenseProcessStepResponse, Is.Not.Null);
                var travelExpenseEntityFromDb = testContext.CreateUnitOfWork()
                    .Repository
                    .List(new TravelExpenseById(testContext.TravelExpenseEntity1.Id))
                    .SingleOrDefault();
                Assert.That(travelExpenseEntityFromDb, Is.Not.Null);
                Assert.That(travelExpenseEntityFromDb.Stage.Value, Is.EqualTo(TravelExpenseStage.ReportedDone));
            }
        }
    }
}