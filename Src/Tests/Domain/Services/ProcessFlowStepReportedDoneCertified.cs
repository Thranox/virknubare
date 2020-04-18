using Domain;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Services;
using Moq;
using NUnit.Framework;

namespace Tests.Domain.Services
{
    public class ProcessFlowStepReportedDoneCertifiedTests
    {
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
            var travelExpenseEntity = new TravelExpenseEntity("", new UserEntity("", ""));
            var processStepStub = new Mock<IProcessFlowStep>();
            processStepStub.Setup(x => x.GetResultingStage(travelExpenseEntity)).Returns(travelExpenseStage);
            travelExpenseEntity.ApplyProcessStep(processStepStub.Object);
            var sut = GetSut();

            // Act & Assert
            Assert.Throws<BusinessRuleViolationException>(() => sut.GetResultingStage(travelExpenseEntity));

        }

        [Test]
        public void GetResultingStage_TravelExpenseInValidStage_ReturnsCertified()
        {
            // Arrange
            var travelExpenseEntity = new TravelExpenseEntity("", new UserEntity("", ""));
            var processStepStub = new Mock<IProcessFlowStep>();
            processStepStub.Setup(x => x.GetResultingStage(travelExpenseEntity)).Returns(TravelExpenseStage.ReportedDone);
            travelExpenseEntity.ApplyProcessStep(processStepStub.Object);
            var sut = GetSut();

            // Act
            var actual = sut.GetResultingStage(travelExpenseEntity);

            // Assert
            Assert.That(actual, Is.EqualTo(TravelExpenseStage.Certified));
        }

        private static ProcessFlowStepReportedDoneCertified GetSut()
        {
            return new ProcessFlowStepReportedDoneCertified();
        }
    }
}//using Domain;
 //using Domain.Entities;
 //using Domain.Exceptions;
 //using Domain.Interfaces;
 //using Domain.SharedKernel;

//namespace Tests.Domain.Services
//{
//    public class ProcessFlowStepReportedDoneCertified : IProcessFlowStep
//    {
//        public bool CanHandle(string key)
//        {
//            return key == Globals.ReporteddoneCertified;
//        }

//        public TravelExpenseStage GetResultingStage(TravelExpenseEntity travelExpenseEntity)
//        {
//            //BR: Can't be certified if not reported done:
//            if (travelExpenseEntity.Stage!=TravelExpenseStage.ReportedDone)
//                throw new BusinessRuleViolationException(travelExpenseEntity.Id, "Rejseafregning kan ikke attesteres da den ikke er færdigmeldt.");

//            travelExpenseEntity.Events.Add(new TravelExpenseUpdatedDomainEvent());

//            return TravelExpenseStage.Certified;
//        }
//    }
//}