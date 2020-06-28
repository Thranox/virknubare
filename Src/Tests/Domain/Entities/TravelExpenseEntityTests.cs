using Domain.Entities;
using Domain.ValueObjects;
using NUnit.Framework;
using Tests.Domain.Services;

namespace Tests.Domain.Entities
{
    public class TravelExpenseEntityTests
    {
        [Test]
        public void CalculatePayoutTable_ValidTe_ReturnsTable()
        {
            // Arrange
            var sut=TestDataHelper.GetValidTravelExpense(new StageEntity(TravelExpenseStage.Initial));

            // Act
            var calculatePayoutTable = sut.CalculatePayoutTable();

            // Assert
            Assert.That(calculatePayoutTable, Is.Not.Null);
        }
    }
}