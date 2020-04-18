using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Application.Services;
using Domain;
using Domain.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Tests.TestHelpers;

namespace Tests.ApplicationServices
{
    public class GetFlowStepServiceIntegrationTests
    {
        [Test]
        public async Task GetAsync_NoParameters_ReturnsFlowSteps()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var sut = testContext.ServiceProvider.GetService<IGetFlowStepService>();

                // Act
                var actual = await sut.GetAsync(TestData.DummyPolSub);

                // Assert
                Assert.That(actual, Is.Not.Null);
                var v = actual.Result.ToArray();
                Assert.That(v.Length, Is.EqualTo(4));
                Assert.That(v,
                    Has.One.EqualTo(new FlowStepDto()
                    {
                        From = TravelExpenseStage.Initial.ToString(),
                        Key = Globals.InitialReporteddone
                    }));
                Assert.That(v,
                    Has.One.EqualTo(new FlowStepDto()
                    {
                        From = TravelExpenseStage.ReportedDone.ToString(),
                        Key = Globals.ReporteddoneCertified
                    }));
                Assert.That(v,
                    Has.One.EqualTo(new FlowStepDto()
                    {
                        From = TravelExpenseStage.Certified.ToString(),
                        Key = Globals.CertifiedAssignedForPayment
                    }));
                Assert.That(v,
                    Has.One.EqualTo(new FlowStepDto()
                    {
                        From = TravelExpenseStage.AssignedForPayment.ToString(),
                        Key = Globals.AssignedForPaymentFinal
                    }));
            }
        }


        //[Test]
        //public async Task GetByIdAsync_IdOfExisting_ReturnsTravelExpense()
        //{
        //    // Arrange
        //    using (var testContext = new IntegrationTestContext())
        //    {
        //        var sut = testContext.ServiceProvider.GetService<IGetTravelExpenseService>();
        //        // Act
        //        var actual = await sut.GetByIdAsync(testContext.TravelExpenseEntity1.Id, Globals.DummyPolSub);

        //        // Assert
        //        Assert.That(actual, Is.Not.Null);
        //        Assert.That(actual.Result, Is.EqualTo(new TravelExpenseDto
        //        {
        //            Description = testContext.TravelExpenseEntity1.Description,
        //            Id = testContext.TravelExpenseEntity1.Id,
        //            Stage = "Initial"
        //        }));
        //    }
        //}

        //[Test]
        //public void GetByIdAsync_IdOfExistingButNotAllowed_ThrowsItemNotAllowedException()
        //{
        //    // Arrange
        //    using (var testContext = new IntegrationTestContext())
        //    {
        //        var sut = testContext.ServiceProvider.GetService<IGetTravelExpenseService>();
        //        // Act & Assert
        //        var itemNotAllowedException = Assert.ThrowsAsync<ItemNotAllowedException>(()=> sut.GetByIdAsync(testContext.TravelExpenseEntity1.Id, Globals.DummyPolSek));
        //        Assert.That(itemNotAllowedException.Id, Is.EqualTo(testContext.TravelExpenseEntity1.Id.ToString()));
        //        Assert.That(itemNotAllowedException.Item, Is.EqualTo("TravelExpense"));
        //    }
        //}
    }
}