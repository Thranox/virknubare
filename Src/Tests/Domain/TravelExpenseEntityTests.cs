using AutoFixture;
using Domain;
using Domain.Entities;
using NUnit.Framework;
using Tests.TestHelpers;

namespace Tests.Domain
{
    public class TravelExpenseEntityTests
    {
        [Test]
        public void Update_InDefaultState_IsAllowed()
        {
            // Arrange
            using (var unitTestContext = new UnitTestContext())
            {
                var description = unitTestContext.Fixture.Create<string>();
                var newDescription = unitTestContext.Fixture.Create<string>();
                var sut = new TravelExpenseEntity(description);

                // Act
                sut.Update(newDescription);

                // Assert
                Assert.That(sut.Description, Is.EqualTo(newDescription));
            }
        }

        [Test]
        public void Update_InReportedDoneState_IsNotAllowed()
        {
            // Arrange
            using (var unitTestContext = new UnitTestContext())
            {
                var description = unitTestContext.Fixture.Create<string>();
                var newDescription = unitTestContext.Fixture.Create<string>();
                var sut = new TravelExpenseEntity(description);
                sut.ReportDone();

                // Act & Assert
                Assert.Throws<BusinessRuleViolationException>(() => sut.Update(newDescription));
            }
        }

        [Test]
        public void Update_InCertifiedState_IsNotAllowed()
        {
            // Arrange
            using (var unitTestContext = new UnitTestContext())
            {
                var description = unitTestContext.Fixture.Create<string>();
                var newDescription = unitTestContext.Fixture.Create<string>();
                var sut = new TravelExpenseEntity(description);
                sut.ReportDone();
                sut.Certify();

                // Act & Assert
                Assert.Throws<BusinessRuleViolationException>(() => sut.Update(newDescription));
            }
        }

        [Test]
        public void Update_InAssignedPaymentState_IsNotAllowed()
        {
            // Arrange
            using (var unitTestContext = new UnitTestContext())
            {
                var description = unitTestContext.Fixture.Create<string>();
                var newDescription = unitTestContext.Fixture.Create<string>();
                var sut = new TravelExpenseEntity(description);
                sut.ReportDone();
                sut.Certify();
                sut.AssignPayment();

                // Act & Assert
                Assert.Throws<BusinessRuleViolationException>(() => sut.Update(newDescription));
            }
        }

        [Test]
        public void ReportDone_InDefaultState_IsAllowed()
        {
            // Arrange
            using (var unitTestContext = new UnitTestContext())
            {
                var description = unitTestContext.Fixture.Create<string>();
                var sut = new TravelExpenseEntity(description);

                // Act
                sut.ReportDone();

                // Assert
                Assert.That(sut.IsReportedDone, Is.EqualTo(true));
            }
        }

        [Test]
        public void ReportDone_InCertifiedState_IsNotAllowed()
        {
            // Arrange
            using (var unitTestContext = new UnitTestContext())
            {
                var description = unitTestContext.Fixture.Create<string>();
                var sut = new TravelExpenseEntity(description);
                sut.ReportDone();
                sut.Certify();

                // Act & Assert
                Assert.Throws<BusinessRuleViolationException>(() => sut.ReportDone());
            }
        }

        [Test]
        public void ReportDone_InAssignedPaymentState_IsNotAllowed()
        {
            // Arrange
            using (var unitTestContext = new UnitTestContext())
            {
                var description = unitTestContext.Fixture.Create<string>();
                var sut = new TravelExpenseEntity(description);
                sut.ReportDone();
                sut.Certify();
                sut.AssignPayment();

                // Act & Assert
                Assert.Throws<BusinessRuleViolationException>(() => sut.ReportDone());
            }
        }

        [Test]
        public void ReportDone_InStateReportedDone_IsNotAllowed()
        {
            // Arrange
            using (var unitTestContext = new UnitTestContext())
            {
                var description = unitTestContext.Fixture.Create<string>();
                var sut = new TravelExpenseEntity(description);
                sut.ReportDone();

                // Act & Assert
                Assert.Throws<BusinessRuleViolationException>(() => sut.ReportDone());
            }
        }

        [Test]
        public void Certify_InDefaultState_IsNotAllowed()
        {
            // Arrange
            using (var unitTestContext = new UnitTestContext())
            {
                var description = unitTestContext.Fixture.Create<string>();
                var sut = new TravelExpenseEntity(description);

                // Act & Assert
                Assert.Throws<BusinessRuleViolationException>(() => sut.Certify());
            }
        }

        [Test]
        public void Certify_InCertifiedState_IsNotAllowed()
        {
            // Arrange
            using (var unitTestContext = new UnitTestContext())
            {
                var description = unitTestContext.Fixture.Create<string>();
                var sut = new TravelExpenseEntity(description);
                sut.ReportDone();
                sut.Certify();

                // Act & Assert
                Assert.Throws<BusinessRuleViolationException>(() => sut.Certify());
            }
        }

        [Test]
        public void Certify_InAssignedPaymentState_IsNotAllowed()
        {
            // Arrange
            using (var unitTestContext = new UnitTestContext())
            {
                var description = unitTestContext.Fixture.Create<string>();
                var sut = new TravelExpenseEntity(description);
                sut.ReportDone();
                sut.Certify();
                sut.AssignPayment();

                // Act & Assert
                Assert.Throws<BusinessRuleViolationException>(() => sut.Certify());
            }
        }

        [Test]
        public void Certify_InReportedDoneState_IsAllowed()
        {
            // Arrange
            using (var unitTestContext = new UnitTestContext())
            {
                var description = unitTestContext.Fixture.Create<string>();
                var sut = new TravelExpenseEntity(description);
                sut.ReportDone();

                // Act
                sut.Certify();

                // Assert
                Assert.That(sut.IsCertified, Is.EqualTo(true));
            }
        }

        [Test]
        public void AssignPayment_InDefaultState_IsNotAllowed()
        {
            // Arrange
            using (var unitTestContext = new UnitTestContext())
            {
                var description = unitTestContext.Fixture.Create<string>();
                var sut = new TravelExpenseEntity(description);

                // Act & Assert
                Assert.Throws<BusinessRuleViolationException>(() => sut.AssignPayment());
            }
        }

        [Test]
        public void AssignPayment_InReportedDoneState_IsNotAllowed()
        {
            // Arrange
            using (var unitTestContext = new UnitTestContext())
            {
                var description = unitTestContext.Fixture.Create<string>();
                var sut = new TravelExpenseEntity(description);
                sut.ReportDone();

                // Act & Assert
                Assert.Throws<BusinessRuleViolationException>(() => sut.AssignPayment());
            }
        }

        [Test]
        public void AssignPayment_InAssignedPaymentState_IsNotAllowed()
        {
            // Arrange
            using (var unitTestContext = new UnitTestContext())
            {
                var description = unitTestContext.Fixture.Create<string>();
                var sut = new TravelExpenseEntity(description);
                sut.ReportDone();
                sut.Certify();
                sut.AssignPayment();

                // Act & Assert
                Assert.Throws<BusinessRuleViolationException>(() => sut.AssignPayment());
            }
        }

        [Test]
        public void AssignPayment_InCertifiedState_IsAllowed()
        {
            // Arrange
            using (var unitTestContext = new UnitTestContext())
            {
                var description = unitTestContext.Fixture.Create<string>();
                var sut = new TravelExpenseEntity(description);
                sut.ReportDone();
                sut.Certify();

                // Act
                sut.AssignPayment();

                // Assert
                Assert.That(sut.IsAssignedPayment, Is.EqualTo(true));
            }
        }
    }
}