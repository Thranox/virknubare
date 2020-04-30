using Domain;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Services;
using Moq;
using NUnit.Framework;

namespace Tests.Domain.Services
{
    public class ProcessFlowStepAssignedForPaymentFinalTests
    {
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
            var travelExpenseEntity = new TravelExpenseEntity("", new UserEntity("", ""), new CustomerEntity(""),stageEntity);
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
            var stageEntity = new StageEntity(TravelExpenseStage.Initial);
            var travelExpenseEntity = new TravelExpenseEntity("", new UserEntity("", ""), new CustomerEntity(""),stageEntity);
            var processStepStub = new Mock<IProcessFlowStep>();
            processStepStub.Setup(x => x.GetResultingStage(travelExpenseEntity))
                .Returns(stageEntity);
            travelExpenseEntity.ApplyProcessStep(processStepStub.Object);
            var sut = GetSut();

            // Act
            var actual = sut.GetResultingStage(travelExpenseEntity);

            // Assert
            Assert.That(actual, Is.EqualTo(TravelExpenseStage.Final));
        }

        private static ProcessFlowStepAssignedForPaymentFinal GetSut()
        {
            return new ProcessFlowStepAssignedForPaymentFinal(new Mock<IStageService>().Object);
        }
    }
}