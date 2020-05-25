using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain.Specifications;
using Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.ApplicationServices
{
    public class ProcessStepTravelExpenseServiceIntegrationTests
    {
        [Test]
        public async Task ProcessStepAsync_ValidTravelExpenseAndStep_Returns()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var flowStepId = testContext
                    .GetUnitOfWork()
                    .Repository
                    .List(
                        new FlowStepByCustomerAndStage(
                            testContext.GetDummyCustomerId(),testContext.TravelExpenseEntity1.Stage.Value
                            )
                        )
                    .Single()
                    .Id;
                var travelExpenseProcessStepDto = new TravelExpenseFlowStepDto()
                {
                    FlowStepId =flowStepId,
                    TravelExpenseId = testContext.TravelExpenseEntity1.Id
                };
                var sut = testContext.ServiceProvider.GetService<IFlowStepTravelExpenseService>();

                // Act
                var travelExpenseProcessStepResponse =await sut.ProcessStepAsync(travelExpenseProcessStepDto, testContext.GetPolApiContext(TestData.DummyPolSubAlice));

                // Assert
                Assert.That(travelExpenseProcessStepResponse, Is.Not.Null);
                var travelExpenseEntityFromDb = testContext.GetUnitOfWork()
                    .Repository
                    .List(new TravelExpenseById(testContext.TravelExpenseEntity1.Id))
                    .SingleOrDefault();
                Assert.That(travelExpenseEntityFromDb, Is.Not.Null);
                Assert.That(travelExpenseEntityFromDb.Stage.Value, Is.EqualTo(TravelExpenseStage.ReportedDone));
            }
        }
    }
}