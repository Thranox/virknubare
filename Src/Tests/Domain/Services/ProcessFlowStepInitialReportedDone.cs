using Domain;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Services;
using Moq;
using NUnit.Framework;

namespace Tests.Domain.Services
{
    public class ProcessFlowStepInitialReportedDoneTests
    {
        [Test]
        public void CanHandle_KeyForInitialReporteddone_ReturnsTrue()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var actual = sut.CanHandle(Globals.InitialReporteddone);

            // Assert
            Assert.That(actual, Is.True);
        }

        [TestCase(TravelExpenseStage.Certified)]
        [TestCase(TravelExpenseStage.AssignedForPayment)]
        [TestCase(TravelExpenseStage.Final)]
        [TestCase(TravelExpenseStage.ReportedDone)]
        public void GetResultingStage_TravelExpenseInInvalidStage_ThrowsBusinessRuleViolationException(TravelExpenseStage travelExpenseStage)
        {
            // Arrange
            var travelExpenseEntity = new TravelExpenseEntity("", new UserEntity("", ""));
            var processStepStub = new Mock<IProcessFlowStep>();
            processStepStub.Setup(x => x.GetResultingStage(travelExpenseEntity)).Returns(travelExpenseStage);
            travelExpenseEntity.ApplyProcessStep(processStepStub.Object);
            var sut = GetSut();

            // Act & Assert
            Assert.Throws<BusinessRuleViolationException>(() => sut.GetResultingStage(travelExpenseEntity));

        }

        [Test]
        public void GetResultingStage_TravelExpenseInValidStage_ReturnsReportedDone()
        {
            // Arrange
            var travelExpenseEntity = new TravelExpenseEntity("", new UserEntity("", ""));
            var sut = GetSut();

            // Act
            var actual = sut.GetResultingStage(travelExpenseEntity);

            // Assert
            Assert.That(actual, Is.EqualTo(TravelExpenseStage.ReportedDone));
        }

        private static ProcessFlowStepInitialReportedDone GetSut()
        {
            return new ProcessFlowStepInitialReportedDone();
        }
    }
}