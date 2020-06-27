using System;
using Domain;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Services;
using Domain.ValueObjects;
using Moq;
using NUnit.Framework;

namespace Tests.Domain.Services
{
    public class ProcessFlowStepAssignedForPaymentFinalTests
    {
        private static Mock<IStageService> _stageServiceMock ;

        [SetUp]
        public void Setup()
        {
            _stageServiceMock = new Mock<IStageService>();
        }

        [Test]
        public void CanHandle_KeyForAssignedForPayment_ReturnsTrue()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var actual = sut.CanHandle(Globals.AssignedForPaymentFinal);

            // Assert
            Assert.That(actual, Is.True);
        }

        [TestCase(TravelExpenseStage.Initial)]
        [TestCase(TravelExpenseStage.Certified)]
        [TestCase(TravelExpenseStage.Final)]
        [TestCase(TravelExpenseStage.ReportedDone)]
        public void GetResultingStage_TravelExpenseInInvalidStage_ThrowsBusinessRuleViolationException(
            TravelExpenseStage travelExpenseStage)
        {
            // Arrange
            var stageEntity = new StageEntity(travelExpenseStage);
            var travelExpenseEntity = TestDataHelper.GetValidTravelExpense(stageEntity);//new TravelExpenseEntity("a", new UserEntity("", ""), new CustomerEntity(""),stageEntity, DateTime.Now);
            var processStepStub = new Mock<IProcessFlowStep>();
            processStepStub.Setup(x => x.GetResultingStage(travelExpenseEntity)).Returns(stageEntity);
            travelExpenseEntity.ApplyProcessStep(processStepStub.Object);
            var sut = GetSut();

            // Act & Assert
            Assert.Throws<BusinessRuleViolationException>(() => sut.GetResultingStage(travelExpenseEntity));
        }

        [Test]
        public void GetResultingStage_TravelExpenseInValidStage_ReturnsFinal()
        {
            // Arrange
            var stageEntityAssignedForPayment = new StageEntity(TravelExpenseStage.AssignedForPayment);
            var travelExpenseEntity = TestDataHelper.GetValidTravelExpense(stageEntityAssignedForPayment);//new TravelExpenseEntity("a", new UserEntity("", ""), new CustomerEntity(""),stageEntityAssignedForPayment, DateTime.Now);
            var processStepStub = new Mock<IProcessFlowStep>();
            processStepStub.Setup(x => x.GetResultingStage(travelExpenseEntity))
                .Returns(stageEntityAssignedForPayment);
            travelExpenseEntity.ApplyProcessStep(processStepStub.Object);
            var stageEntityFinal = new StageEntity(TravelExpenseStage.Final);
            _stageServiceMock.Setup(x => x.GetStage(TravelExpenseStage.Final))
                .Returns(stageEntityFinal);
            var sut = GetSut();

            // Act
            var actual = sut.GetResultingStage(travelExpenseEntity);

            // Assert
            Assert.That(actual.Value, Is.EqualTo(TravelExpenseStage.Final));
        }

        private static ProcessFlowStepAssignedForPaymentFinal GetSut()
        {
            return new ProcessFlowStepAssignedForPaymentFinal(_stageServiceMock.Object);
        }
    }
}