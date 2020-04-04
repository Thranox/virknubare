using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Web.ApiModels;
using Web.Controllers;

namespace Tests.Web
{
    public class TravelExpenseControllerIntegrationTests
    {
        [Test]
        public async Task Get_NoParameters_ReturnsTravelExpenses()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            using (var context = new PolDbContext(testContext.DbContextOptions))
            {
                var logger = new Logger<TravelExpenseController>(testContext.LoggerFactory);
                var efRepository = new EfRepository(context);
                var sut = new TravelExpenseController(logger, efRepository, testContext.Mapper);

                // Act
                var actual = await sut.Get();

                // Assert
                var travelExpenseDtos = actual.ToArray();
                Assert.That(travelExpenseDtos.Length, Is.EqualTo(3));
                Assert.That(travelExpenseDtos, Has.One.EqualTo(new TravelExpenseDto {Description = "Expense1"}));
                Assert.That(travelExpenseDtos, Has.One.EqualTo(new TravelExpenseDto {Description = "Expense2"}));
                Assert.That(travelExpenseDtos, Has.One.EqualTo(new TravelExpenseDto {Description = "Expense3"}));
            }
        }
    }
}