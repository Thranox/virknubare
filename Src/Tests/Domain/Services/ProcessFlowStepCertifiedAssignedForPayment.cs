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
        private static Mock<IStageService> _stageServiceMock;

        [SetUp]
        public void Setup()
        {
            _stageServiceMock = new Mock<IStageService>();
        }

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
            var travelExpenseEntity = new TravelExpenseEntity("a", new UserEntity("", ""), new CustomerEntity(""),stageEntity);
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
            var stageEntityCertified = new StageEntity(TravelExpenseStage.Certified);
            var travelExpenseEntity = new TravelExpenseEntity("a", new UserEntity("", ""), new CustomerEntity(""),stageEntityCertified);
            var processStepStub = new Mock<IProcessFlowStep>();
            processStepStub.Setup(x => x.GetResultingStage(travelExpenseEntity)).Returns(stageEntityCertified);
            travelExpenseEntity.ApplyProcessStep(processStepStub.Object);
            var stageEntityAssignedForPayment = new StageEntity(TravelExpenseStage.AssignedForPayment);
            _stageServiceMock.Setup(x => x.GetStage(TravelExpenseStage.AssignedForPayment))
                .Returns(stageEntityAssignedForPayment);

            var sut = GetSut();

            // Act
            var actual = sut.GetResultingStage(travelExpenseEntity);

            // Assert
            Assert.That(actual.Value, Is.EqualTo(TravelExpenseStage.AssignedForPayment));
        }

        private static ProcessFlowStepCertifiedAssignedForPayment GetSut()
        {
            return new ProcessFlowStepCertifiedAssignedForPayment(_stageServiceMock.Object);
        }
    }
}