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
        private static Mock<IStageService> _stageServiceMock;

        [SetUp]
        public void Setup()
        {
            _stageServiceMock = new Mock<IStageService>();
        }

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
            var stageEntity = new StageEntity(travelExpenseStage);
            var travelExpenseEntity = new TravelExpenseEntity("", new UserEntity("", ""), new CustomerEntity(""), stageEntity);
            var processStepStub = new Mock<IProcessFlowStep>();
            processStepStub.Setup(x => x.GetResultingStage(travelExpenseEntity)).Returns(stageEntity);
            travelExpenseEntity.ApplyProcessStep(processStepStub.Object);
            var sut = GetSut();

            // Act & Assert
            Assert.Throws<BusinessRuleViolationException>(() => sut.GetResultingStage(travelExpenseEntity));

        }

        [Test]
        public void GetResultingStage_TravelExpenseInValidStage_ReturnsReportedDone()
        {
            // Arrange
            var stageEntity = new StageEntity(TravelExpenseStage.Initial);
            var travelExpenseEntity = new TravelExpenseEntity("", new UserEntity("", ""), new CustomerEntity(""),stageEntity);
            var stageEntityreportedDone=new StageEntity(TravelExpenseStage.ReportedDone);
            _stageServiceMock.Setup(x => x.GetStage(TravelExpenseStage.ReportedDone))
                .Returns(stageEntityreportedDone);

            var sut = GetSut();

            // Act
            var actual = sut.GetResultingStage(travelExpenseEntity);

            // Assert
            Assert.That(actual.Value, Is.EqualTo(TravelExpenseStage.ReportedDone));
        }

        private static ProcessFlowStepInitialReportedDone GetSut()
        {
            return new ProcessFlowStepInitialReportedDone(_stageServiceMock.Object);
        }
    }
}