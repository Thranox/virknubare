using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using Domain.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Tests.TestHelpers;

namespace Tests.ApplicationServices
{
    public class GetTravelExpenseServiceIntegrationTests
    {
        [Test]
        public async Task GetAsync_NoParameters_ReturnsTravelExpenses()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = testContext.ServiceProvider.GetService<IGetTravelExpenseService>();
                // Act
                var actual = await sut.GetAsync(Globals.DummyPolSub);

                // Assert
                Assert.That(actual, Is.Not.Null);
                var v = actual.Result.ToArray();
                Assert.That(v.Length, Is.EqualTo(3));
                Assert.That(v,
                    Has.One.EqualTo(new TravelExpenseDto
                    {
                        Description = testContext.TravelExpenseEntity1.Description,
                        Id = testContext.TravelExpenseEntity1.Id,
                        Stage = "Initial"
                    }));
                Assert.That(v,
                    Has.One.EqualTo(new TravelExpenseDto
                    {
                        Description = testContext.TravelExpenseEntity2.Description,
                        Id = testContext.TravelExpenseEntity2.Id,
                        Stage = "Initial"
                    }));
                Assert.That(v,
                    Has.One.EqualTo(new TravelExpenseDto
                    {
                        Description = testContext.TravelExpenseEntity3.Description,
                        Id = testContext.TravelExpenseEntity3.Id,
                        Stage = "Initial"
                    }));
            }
        }

        [Test]
        public async Task GetByIdAsync_IdOfExisting_ReturnsTravelExpense()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = testContext.ServiceProvider.GetService<IGetTravelExpenseService>();
                // Act
                var actual = await sut.GetByIdAsync(testContext.TravelExpenseEntity1.Id, Globals.DummyPolSub);

                // Assert
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual.Result, Is.EqualTo(new TravelExpenseDto
                {
                    Description = testContext.TravelExpenseEntity1.Description,
                    Id = testContext.TravelExpenseEntity1.Id,
                    Stage = "Initial"
                }));
            }
        }

        [Test]
        public void GetByIdAsync_IdOfExistingButNotAllowed_ThrowsItemNotAllowedException()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = testContext.ServiceProvider.GetService<IGetTravelExpenseService>();
                // Act & Assert
                var itemNotAllowedException = Assert.ThrowsAsync<ItemNotAllowedException>(()=> sut.GetByIdAsync(testContext.TravelExpenseEntity1.Id, Globals.DummyPolSek));
                Assert.That(itemNotAllowedException.Id, Is.EqualTo(testContext.TravelExpenseEntity1.Id.ToString()));
                Assert.That(itemNotAllowedException.Item, Is.EqualTo("TravelExpense"));
            }
        }
    }
}