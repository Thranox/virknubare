using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using Domain.Entities;
using Domain.Exceptions;
using Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.ApplicationServices
{
    public class GetTravelExpenseServiceIntegrationTests
    {
        [Test]
        public async Task GetAsync_NoParameters_ReturnsTravelExpenses()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = testContext.ServiceProvider.GetService<IGetTravelExpenseService>();
                // Act
                var actual = await sut.GetAsync(TestData.DummyPolSubAlice);

                // Assert
                Assert.That(actual, Is.Not.Null);
                var v = actual.Result.ToArray();
                Assert.That(v.Length, Is.EqualTo(3));

                var stageEntities = testContext.CreateUnitOfWork().Repository.List<StageEntity>().ToArray();
                var flowSteps = testContext.CreateUnitOfWork().Repository.List<FlowStepEntity>().ToArray();
                var flowStepId = flowSteps.Single(x => x.From.Value == TravelExpenseStage.Initial).Id;

                Assert.That(v,
                    Has.One.EqualTo(new TravelExpenseDto
                    {
                        Description = testContext.TravelExpenseEntity1.Description,
                        Id = testContext.TravelExpenseEntity1.Id,
                        StageId = stageEntities.Single(x => x.Value == TravelExpenseStage.Initial).Id.ToString(),
                        StageText = Globals.StageNamesDanish[TravelExpenseStage.Initial],
                        AllowedFlows = new[] { new AllowedFlowDto { Description = "Færdigmeld", FlowStepId = flowStepId } }
                    }));
                Assert.That(v,
                    Has.One.EqualTo(new TravelExpenseDto
                    {
                        Description = testContext.TravelExpenseEntity2.Description,
                        Id = testContext.TravelExpenseEntity2.Id,
                        StageId = stageEntities.Single(x => x.Value == TravelExpenseStage.Initial).Id.ToString(),
                        StageText = Globals.StageNamesDanish[TravelExpenseStage.Initial],
                        AllowedFlows = new[] { new AllowedFlowDto { Description = "Færdigmeld", FlowStepId = flowStepId } }
                    }));
                Assert.That(v,
                    Has.One.EqualTo(new TravelExpenseDto
                    {
                        Description = testContext.TravelExpenseEntity3.Description,
                        Id = testContext.TravelExpenseEntity3.Id,
                        StageId = stageEntities.Single(x => x.Value == TravelExpenseStage.Initial).Id.ToString(),
                        StageText = Globals.StageNamesDanish[TravelExpenseStage.Initial],
                        AllowedFlows = new[] { new AllowedFlowDto { Description = "Færdigmeld", FlowStepId = flowStepId } }
                    }));
            }
        }

        [Test]
        public void GetAsync_NonExistingUser_Throws()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = testContext.ServiceProvider.GetService<IGetTravelExpenseService>();
                
                // Act
                var itemNotFoundException = Assert.ThrowsAsync<ItemNotFoundException>(() => sut.GetAsync(Guid.Empty.ToString()));
                Assert.That(itemNotFoundException.Id, Is.EqualTo(Guid.Empty.ToString()));
                Assert.That(itemNotFoundException.Item, Is.EqualTo("UserEntity"));
            }
        }

        [Test]
        public async Task GetByIdAsync_IdOfExisting_ReturnsTravelExpense()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = testContext.ServiceProvider.GetService<IGetTravelExpenseService>();
                // Act
                var actual = await sut.GetByIdAsync(testContext.TravelExpenseEntity1.Id, TestData.DummyPolSubAlice);

                // Assert
                Assert.That(actual, Is.Not.Null);

                var stageEntities = testContext.CreateUnitOfWork().Repository.List<StageEntity>().ToArray();
                var flowSteps = testContext.CreateUnitOfWork().Repository.List<FlowStepEntity>().ToArray();
                var flowStepId = flowSteps.Single(x => x.From.Value == TravelExpenseStage.Initial).Id;

                Assert.That(actual.Result, Is.EqualTo(new TravelExpenseDto
                {
                    Description = testContext.TravelExpenseEntity1.Description,
                    Id = testContext.TravelExpenseEntity1.Id,
                    StageId = stageEntities.Single(x => x.Value == TravelExpenseStage.Initial).Id.ToString(),
                    StageText = Globals.StageNamesDanish[TravelExpenseStage.Initial],
                    AllowedFlows = new[] { new AllowedFlowDto { Description = "Færdigmeld", FlowStepId = flowStepId } }
                }));
            }
        }

        [Test]
        public void GetByIdAsync_IdOfNonExistingUser_Throws()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = testContext.ServiceProvider.GetService<IGetTravelExpenseService>();
                // Act & Assert
                var itemNotFoundException = Assert.ThrowsAsync<ItemNotFoundException>(() =>
                    sut.GetByIdAsync(testContext.TravelExpenseEntity1.Id, Guid.Empty.ToString()));
                Assert.That(itemNotFoundException.Id, Is.EqualTo(Guid.Empty.ToString()));
                Assert.That(itemNotFoundException.Item, Is.EqualTo("UserEntity"));
            }
        }

        [Test]
        public void GetByIdAsync_IdOfNonExistingTravelExpense_Throws()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = testContext.ServiceProvider.GetService<IGetTravelExpenseService>();
                // Act & Assert
                var itemNotFoundException = Assert.ThrowsAsync<ItemNotFoundException>(() =>
                    sut.GetByIdAsync(Guid.Empty, TestData.DummyPolSubAlice));
                Assert.That(itemNotFoundException.Id, Is.EqualTo(Guid.Empty.ToString()));
                Assert.That(itemNotFoundException.Item, Is.EqualTo("TravelExpenseEntity"));
            }
        }

        [Test]
        public void GetByIdAsync_IdOfExistingButNotAllowed_ThrowsItemNotAllowedException()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = testContext.ServiceProvider.GetService<IGetTravelExpenseService>();

                // Act & Assert
                var itemNotAllowedException = Assert.ThrowsAsync<ItemNotAllowedException>(() =>
                    sut.GetByIdAsync(testContext.TravelExpenseEntity1.Id, TestData.DummySekSubBob));
                Assert.That(itemNotAllowedException.Id, Is.EqualTo(testContext.TravelExpenseEntity1.Id.ToString()));
                Assert.That(itemNotAllowedException.Item, Is.EqualTo("TravelExpenseEntity"));
            }
        }
    }
}