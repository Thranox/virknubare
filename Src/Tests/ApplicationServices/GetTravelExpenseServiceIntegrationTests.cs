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
using TestHelpers;
using Tests.TestHelpers;

namespace Tests.ApplicationServices
{
    public class GetTravelExpenseServiceIntegrationTests
    {
        [Test]
        public async Task GetAsync_NoParameters_ReturnsTravelExpenses()
        {
            // Arrange
            using var testContext = new IntegrationTestContext();
            
            var sut = testContext.ServiceProvider.GetService<IGetTravelExpenseService>();

            // Act
            var actual = await sut.GetAsync(testContext.GetPolApiContext(TestData.DummyPolSubAlice));

            // Assert
            Assert.That(actual, Is.Not.Null);
            var v = actual.Result.ToArray();
            Assert.That(v.Length, Is.EqualTo(TestData.GetNumberOfTestDataTravelExpenses()));

            var stageEntities = testContext.GetUnitOfWork().Repository.List<StageEntity>().ToArray();
            var flowSteps = testContext.GetUnitOfWork().Repository.List<FlowStepEntity>().ToArray();
            var flowStepId = flowSteps.Single(x => x.From.Value == TravelExpenseStage.Initial).Id;

            Assert.That(v,
                Has.One.EqualTo(new TravelExpenseDto
                {
                    Description = testContext.TravelExpenseEntity1.Description,
                    Id = testContext.TravelExpenseEntity1.Id,
                    StageId = stageEntities.Single(x => x.Value == TravelExpenseStage.Initial).Id.ToString(),
                    StageText = Globals.StageNamesDanish[TravelExpenseStage.Initial],
                    AllowedFlows = new[] {new AllowedFlowDto {Description = "Færdigmeld", FlowStepId = flowStepId}}
                }));
            Assert.That(v,
                Has.One.EqualTo(new TravelExpenseDto
                {
                    Description = testContext.TravelExpenseEntity2.Description,
                    Id = testContext.TravelExpenseEntity2.Id,
                    StageId = stageEntities.Single(x => x.Value == TravelExpenseStage.Initial).Id.ToString(),
                    StageText = Globals.StageNamesDanish[TravelExpenseStage.Initial],
                    AllowedFlows = new[] {new AllowedFlowDto {Description = "Færdigmeld", FlowStepId = flowStepId}}
                }));
            Assert.That(v,
                Has.One.EqualTo(new TravelExpenseDto
                {
                    Description = testContext.TravelExpenseEntity3.Description,
                    Id = testContext.TravelExpenseEntity3.Id,
                    StageId = stageEntities.Single(x => x.Value == TravelExpenseStage.Initial).Id.ToString(),
                    StageText = Globals.StageNamesDanish[TravelExpenseStage.Initial],
                    AllowedFlows = new[] {new AllowedFlowDto {Description = "Færdigmeld", FlowStepId = flowStepId}}
                }));
        }

        [Test]
        public async Task GetByIdAsync_IdOfExisting_ReturnsTravelExpense()
        {
            // Arrange
            using var testContext = new IntegrationTestContext();
            
            var sut = testContext.ServiceProvider.GetService<IGetTravelExpenseService>();
            // Act
            var actual = await sut.GetByIdAsync(testContext.GetPolApiContext(TestData.DummyPolSubAlice),
                testContext.TravelExpenseEntity1.Id);

            // Assert
            Assert.That(actual, Is.Not.Null);

            var stageEntities = testContext.GetUnitOfWork().Repository.List<StageEntity>().ToArray();
            var flowSteps = testContext.GetUnitOfWork().Repository.List<FlowStepEntity>().ToArray();
            var flowStepId = flowSteps.Single(x => x.From.Value == TravelExpenseStage.Initial).Id;

            Assert.That(actual.Result, Is.EqualTo(new TravelExpenseSingleDto
            {
                Description = testContext.TravelExpenseEntity1.Description,
                Id = testContext.TravelExpenseEntity1.Id,
                StageId = stageEntities.Single(x => x.Value == TravelExpenseStage.Initial).Id.ToString(),
                StageText = Globals.StageNamesDanish[TravelExpenseStage.Initial],
                AllowedFlows = new[] {new AllowedFlowDto {Description = "Færdigmeld", FlowStepId = flowStepId}}
            }));
        }

        [Test]
        public void GetByIdAsync_IdOfNonExistingTravelExpense_Throws()
        {
            // Arrange
            using var testContext = new IntegrationTestContext();

            var sut = testContext.ServiceProvider.GetService<IGetTravelExpenseService>();
            // Act & Assert
            var itemNotFoundException = Assert.ThrowsAsync<ItemNotFoundException>(() =>
                sut.GetByIdAsync(testContext.GetPolApiContext(TestData.DummyPolSubAlice), Guid.Empty));
            Assert.That(itemNotFoundException.Id, Is.EqualTo(Guid.Empty.ToString()));
            Assert.That(itemNotFoundException.Item, Is.EqualTo("TravelExpenseEntity"));
        }

        [Test]
        public void GetByIdAsync_IdOfExistingButNotAllowed_ThrowsItemNotAllowedException()
        {
            // Arrange
            using var testContext = new IntegrationTestContext();

            var sut = testContext.ServiceProvider.GetService<IGetTravelExpenseService>();

            // Act & Assert
            var itemNotAllowedException = Assert.ThrowsAsync<ItemNotAllowedException>(() =>
                sut.GetByIdAsync(testContext.GetPolApiContext(TestData.DummySekSubBob),
                    testContext.TravelExpenseEntity1.Id));
            Assert.That(itemNotAllowedException.Id, Is.EqualTo(testContext.TravelExpenseEntity1.Id.ToString()));
            Assert.That(itemNotAllowedException.Item, Is.EqualTo("TravelExpenseEntity"));
        }
    }
}