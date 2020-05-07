using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Application.Services;
using Domain;
using Domain.Exceptions;
using Domain.Specifications;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SharedWouldBeNugets;
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
                var actual = await sut.GetAsync(TestData.DummyPolSubAlice);

                // Assert
                Assert.That(actual, Is.Not.Null);
                var resultArray = actual.Result.ToArray();
                Assert.That(resultArray.Length, Is.EqualTo(4));
                var customer = testContext
                    .CreateUnitOfWork()
                    .Repository
                    .List(new CustomerByName(TestData.DummyCustomerName))
                    .Single();

                var stageEntityInitial = testContext
                    .CreateUnitOfWork()
                    .Repository
                    .List(new StageByValue(TravelExpenseStage.Initial))
                    .Single();

                var stageEntityReportedDone = testContext
                    .CreateUnitOfWork()
                    .Repository
                    .List(new StageByValue(TravelExpenseStage.ReportedDone))
                    .Single();

                var stageEntityCertified = testContext
                    .CreateUnitOfWork()
                    .Repository
                    .List(new StageByValue(TravelExpenseStage.Certified))
                    .Single();

                var stageEntityAssignedForPayment = testContext
                    .CreateUnitOfWork()
                    .Repository
                    .List(new StageByValue(TravelExpenseStage.AssignedForPayment))
                    .Single();

                var stageEntityFinal = testContext
                    .CreateUnitOfWork()
                    .Repository
                    .List(new StageByValue(TravelExpenseStage.Final))
                    .Single();

                Assert.That(resultArray,
                    Has.One.EqualTo(new FlowStepDto()
                    {
                        FromStageId = stageEntityInitial.Id,
                        FromStageText = Globals.StageNamesDanish[TravelExpenseStage.Initial],
                        Key = Globals.InitialReporteddone,
                        CustomerName = customer.Name,
                        CustomerId = customer.Id
                    }));
                Assert.That(resultArray,
                    Has.One.EqualTo(new FlowStepDto()
                    {
                        FromStageId = stageEntityReportedDone.Id,
                        FromStageText = Globals.StageNamesDanish[TravelExpenseStage.ReportedDone],
                        Key = Globals.ReporteddoneCertified,
                        CustomerName = customer.Name,
                        CustomerId = customer.Id,
                    }));
                Assert.That(resultArray,
                    Has.One.EqualTo(new FlowStepDto()
                    {
                        FromStageId = stageEntityCertified.Id,
                        FromStageText = Globals.StageNamesDanish[TravelExpenseStage.Certified],
                        Key = Globals.CertifiedAssignedForPayment,
                        CustomerName = customer.Name,
                        CustomerId = customer.Id,
                    }));
                Assert.That(resultArray,
                    Has.One.EqualTo(new FlowStepDto()
                    {
                        FromStageId = stageEntityAssignedForPayment.Id,
                        FromStageText = Globals.StageNamesDanish[TravelExpenseStage.AssignedForPayment],
                        Key = Globals.AssignedForPaymentFinal,
                        CustomerName = customer.Name,
                        CustomerId = customer.Id,
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
        //        var actual = await sut.GetByIdAsync(testContext.TravelExpenseEntity1.Id, Globals.DummyPolSubAlice);

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