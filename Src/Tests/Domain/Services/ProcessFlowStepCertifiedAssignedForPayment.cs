using Domain;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Services;
using Moq;
using NUnit.Framework;

namespace Tests.Domain.Services
{
    public class ProcessFlowStepCertifiedAssignedForPaymentTests
    {
        [Test]
        public void CanHandle_KeyForCertifiedAssignedForPayment_ReturnsTrue()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var actual = sut.CanHandle(Globals.CertifiedAssignedForPayment);

            // Assert
            Assert.That(actual, Is.True);
        }

        [TestCase(TravelExpenseStage.Initial)]
        [TestCase(TravelExpenseStage.AssignedForPayment)]
        [TestCase(TravelExpenseStage.Final)]
        [TestCase(TravelExpenseStage.ReportedDone)]
        public void GetResultingStage_TravelExpenseInInvalidStage_ThrowsBusinessRuleViolationException(TravelExpenseStage travelExpenseStage)
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
        public void GetResultingStage_TravelExpenseInValidStage_ReturnsAssignedForPayment()
        {
            // Arrange
            var stageEntity = new StageEntity(TravelExpenseStage.Initial);
            var travelExpenseEntity = new TravelExpenseEntity("", new UserEntity("", ""), new CustomerEntity(""),stageEntity);
            var processStepStub = new Mock<IProcessFlowStep>();
            processStepStub.Setup(x => x.GetResultingStage(travelExpenseEntity)).Returns(stageEntity);
            travelExpenseEntity.ApplyProcessStep(processStepStub.Object);
            var sut = GetSut();

            // Act
            var actual = sut.GetResultingStage(travelExpenseEntity);

            // Assert
            Assert.That(actual, Is.EqualTo(TravelExpenseStage.AssignedForPayment));
        }

        private static ProcessFlowStepCertifiedAssignedForPayment GetSut()
        {
            return new ProcessFlowStepCertifiedAssignedForPayment(new Mock<IStageService>().Object);
        }
    }
}