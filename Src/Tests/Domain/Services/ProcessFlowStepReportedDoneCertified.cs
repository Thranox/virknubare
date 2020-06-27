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
    public class ProcessFlowStepReportedDoneCertifiedTests
    {
        private static Mock<IStageService> _stageServiceMock;

        [SetUp]
        public void Setup()
        {
            _stageServiceMock = new Mock<IStageService>();
        }

        [Test]
        public void CanHandle_KeyForReporteddoneCertified_ReturnsTrue()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var actual = sut.CanHandle(Globals.ReporteddoneCertified);

            // Assert
            Assert.That(actual, Is.True);
        }

        [TestCase(TravelExpenseStage.Initial)]
        [TestCase(TravelExpenseStage.AssignedForPayment)]
        [TestCase(TravelExpenseStage.Final)]
        [TestCase(TravelExpenseStage.Certified)]
        public void GetResultingStage_TravelExpenseInInvalidStage_ThrowsBusinessRuleViolationException(TravelExpenseStage travelExpenseStage)
        {
            // Arrange
            var stageEntity = new StageEntity(travelExpenseStage);
            var travelExpenseEntity =TestDataHelper.GetValidTravelExpense(stageEntity);
            var processStepStub = new Mock<IProcessFlowStep>();
            processStepStub.Setup(x => x.GetResultingStage(travelExpenseEntity)).Returns(stageEntity);
            travelExpenseEntity.ApplyProcessStep(processStepStub.Object);
            var sut = GetSut();

            // Act & Assert
            Assert.Throws<BusinessRuleViolationException>(() => sut.GetResultingStage(travelExpenseEntity));

        }

        [Test]
        public void GetResultingStage_TravelExpenseInValidStage_ReturnsCertified()
        {
            // Arrange
            var stageEntity = new StageEntity(TravelExpenseStage.ReportedDone);
            var travelExpenseEntity = TestDataHelper.GetValidTravelExpense(stageEntity);
            var processStepStub = new Mock<IProcessFlowStep>();
            processStepStub.Setup(x => x.GetResultingStage(travelExpenseEntity)).Returns(stageEntity);
            travelExpenseEntity.ApplyProcessStep(processStepStub.Object);
            var stageEntityCertified=new StageEntity(TravelExpenseStage.Certified);
            _stageServiceMock.Setup(x => x.GetStage(TravelExpenseStage.Certified))
                .Returns(stageEntityCertified);

            var sut = GetSut();

            // Act
            var actual = sut.GetResultingStage(travelExpenseEntity);

            // Assert
            Assert.That(actual.Value, Is.EqualTo(TravelExpenseStage.Certified));
        }

        private static ProcessFlowStepReportedDoneCertified GetSut()
        {
            return new ProcessFlowStepReportedDoneCertified(_stageServiceMock.Object);
        }
    }
}