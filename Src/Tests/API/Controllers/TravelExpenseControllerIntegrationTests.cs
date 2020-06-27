using System;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Controllers;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using AutoFixture;
using Domain;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Specifications;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using TestHelpers;
using Tests.Domain.Services;
using Tests.TestHelpers;

namespace Tests.API.Controllers
{
    public class TravelExpenseControllerIntegrationTests
    {
        private static Mock<ISubManagementService> _subManagementService;

        [SetUp]
        public void SetUp()
        {
            _subManagementService = new Mock<ISubManagementService>();
        }

        [Test]
        public async Task Get_NoParameters_ReturnsTravelExpenses()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                _subManagementService.Setup(x => x.GetPolApiContext(It.IsAny<HttpContext>()))
                    .ReturnsAsync(testContext.GetPolApiContext(TestData.DummyPolSubAlice));

                var sut = GetSut(testContext);

                // Act
                var actual = await sut.Get();

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as TravelExpenseGetResponse;
                Assert.That(value, Is.Not.Null);
                var v = value.Result.ToArray();
                Assert.That(v.Length, Is.EqualTo(TestData.GetNumberOfTestDataTravelExpenses()));

                var stageEntities = testContext.GetUnitOfWork().Repository.List<StageEntity>().ToArray();
                var flowSteps = testContext.GetUnitOfWork().Repository.List<FlowStepEntity>().ToArray();
                var flowStepId = flowSteps.Single(x=>x.From.Value==TravelExpenseStage.Initial).Id;

                Assert.That(v[0],Is.EqualTo(new TravelExpenseDto
                    {
                        Description = testContext.TravelExpenseEntity1.Description,
                        Id = testContext.TravelExpenseEntity1.Id,
                        StageId = stageEntities.Single(x=>x.Value==TravelExpenseStage.Initial).Id.ToString(),
                        StageText =Globals.StageNamesDanish[TravelExpenseStage.Initial],
                        AllowedFlows = new[] {new AllowedFlowDto {Description = "Færdigmeld", FlowStepId = flowStepId} }
                    }));
                Assert.That(v,
                    Has.One.EqualTo(new TravelExpenseDto
                    {
                        Description = testContext.TravelExpenseEntity2.Description,
                        Id = testContext.TravelExpenseEntity2.Id,
                        StageId = stageEntities.Single(x => x.Value == TravelExpenseStage.Initial).Id.ToString(),
                        StageText = Globals.StageNamesDanish[TravelExpenseStage.Initial],
                        AllowedFlows = new[] { new AllowedFlowDto { Description = "Færdigmeld", FlowStepId = flowStepId } }
                    }));
                Assert.That(v,
                    Has.One.EqualTo(new TravelExpenseDto
                    {
                        Description = testContext.TravelExpenseEntity3.Description,
                        Id = testContext.TravelExpenseEntity3.Id,
                        StageId = stageEntities.Single(x => x.Value == TravelExpenseStage.Initial).Id.ToString(),
                        StageText = Globals.StageNamesDanish[TravelExpenseStage.Initial],
                        AllowedFlows = new[] { new AllowedFlowDto { Description = "Færdigmeld", FlowStepId = flowStepId } }
                    }));
            }
        }

        [Test]
        public async Task Put_ExistingTravelExpenseWithChanges_ReturnsOk()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                ActionResult<TravelExpenseUpdateResponse> actual;
                var newDescription = testContext.Fixture.Create<string>();
                Guid existingId;
                using (var unitOfWork = testContext.GetUnitOfWork())
                {
                    var existing = unitOfWork.Repository.List<TravelExpenseEntity>().First();
                    existingId = existing.Id;
                    var travelExpenseUpdateDto = new TravelExpenseUpdateDto
                        {Description = newDescription};
                    var sut = GetSut(testContext);

                    // Act
                    actual = await sut.Put(existingId, travelExpenseUpdateDto);
                }

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as TravelExpenseUpdateResponse;
                Assert.That(value, Is.Not.Null);
                using (var unitOfWork = testContext.GetUnitOfWork())
                {
                    var travelExpenseEntity = unitOfWork
                        .Repository
                        .List(new TravelExpenseById(existingId))
                        .Single();
                    Assert.That(travelExpenseEntity.Description, Is.EqualTo(newDescription));
                }
            }
        }

        [Test]
        public void Put_NonExistingTravelExpenseWithChanges_ThrowsTravelExpenseNotFoundByIdException()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var newDescription = testContext.Fixture.Create<string>();
                var existingId = Guid.NewGuid();
                var travelExpenseUpdateDto = new TravelExpenseUpdateDto {Description = newDescription};

                var sut = GetSut(testContext);

                // Act
                var travelExpenseNotFoundByIdException =
                    Assert.ThrowsAsync<ItemNotFoundException>(async () => await sut.Put(existingId, travelExpenseUpdateDto));

                // Assert
                Assert.That(travelExpenseNotFoundByIdException, Is.Not.Null);
                Assert.That(travelExpenseNotFoundByIdException.Id, Is.EqualTo(existingId.ToString()));
            }
        }

        [Test]
        public void Put_ExistingTravelExpenseButUpdateViolatesBusinessRule_ThrowsBusinessRuleViolationException()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var newDescription = string.Empty;
                using (var unitOfWork = testContext.ServiceProvider.GetService<IUnitOfWork>())
                {
                    var existing = unitOfWork.Repository.List<TravelExpenseEntity>().First();
                    existing.ApplyProcessStep(testContext.ServiceProvider.GetServices<IProcessFlowStep>().Single(x=>x.CanHandle(Globals.InitialReporteddone)));
                    var travelExpenseUpdateDto = new TravelExpenseUpdateDto
                        {Description = newDescription };
                    var sut = GetSut(testContext);

                    // Act
                    var businessRuleViolationException =
                        Assert.ThrowsAsync<BusinessRuleViolationException>(async () =>
                            await sut.Put(existing.Id, travelExpenseUpdateDto));

                    // Assert
                    Assert.That(businessRuleViolationException, Is.Not.Null);
                    Assert.That(businessRuleViolationException.EntityId, Is.EqualTo(existing.Id));
                }
            }
        }

        //[Test]
        //public async Task Certify_ExistingTravelExpense_ReturnsOk()
        //{
        //    // Arrange
        //    using (var testContext = new IntegrationTestContext())
        //    {
        //        ActionResult<TravelExpenseCertifyResponse> actual;
        //        Guid existingId;
        //        using (var unitOfWork = testContext.GetUnitOfWork())
        //        {
        //            var existing = unitOfWork.Repository.List<TravelExpenseEntity>().First();
        //            //existing.ReportDone();
        //            existingId = existing.Id;
        //            unitOfWork.Commit();
        //        }

        //        using (var unitOfWork = testContext.GetUnitOfWork())
        //        {
        //            var travelExpenseApproveDto = new TravelExpenseCertifyDto {Id = existingId};
        //            var sut = GetSut(testContext, unitOfWork);

        //            // Act
        //            actual = await sut.Certify(travelExpenseApproveDto);
        //        }

        //        // Assert
        //        Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
        //        var okObjectResult = actual.Result as OkObjectResult;
        //        Assert.That(okObjectResult, Is.Not.Null);
        //        var value = okObjectResult.Value as TravelExpenseCertifyResponse;
        //        Assert.That(value, Is.Not.Null);
        //        using (var unitOfWork = testContext.GetUnitOfWork())
        //        {
        //            var travelExpenseEntity = unitOfWork
        //                .Repository
        //                .List(new TravelExpenseById(existingId))
        //                .Single();
        //            Assert.That(travelExpenseEntity.IsCertified, Is.EqualTo(true));
        //        }
        //    }
        //}

        //[Test]
        //public void Certify_NonExistingTravelExpense_ThrowsTravelExpenseNotFoundByIdException()
        //{
        //    // Arrange
        //    using (var testContext = new IntegrationTestContext())
        //    {
        //        var existingId = Guid.NewGuid();
        //        using (var unitOfWork = testContext.GetUnitOfWork())
        //        {
        //            var travelExpenseCertifyDto = new TravelExpenseCertifyDto {Id = existingId};
        //            var sut = GetSut(testContext, unitOfWork);

        //            // Act
        //            Assert.ThrowsAsync<ItemNotFoundException>(() => sut.Certify(travelExpenseCertifyDto));
        //        }
        //    }
        //}

        //[Test]
        //public async Task ReportDone_ExistingTravelExpense_ReturnsOk()
        //{
        //    // Arrange
        //    using (var testContext = new IntegrationTestContext())
        //    {
        //        ActionResult<TravelExpenseReportDoneResponse> actual;
        //        Guid existingId;
        //        using (var unitOfWork = testContext.GetUnitOfWork())
        //        {
        //            var existing = unitOfWork.Repository.List<TravelExpenseEntity>().First();
        //            existingId = existing.Id;
        //            var travelExpenseReportDoneDto = new TravelExpenseReportDoneDto {Id = existingId};
        //            var sut = GetSut(testContext, unitOfWork);

        //            // Act
        //            actual = await sut.ReportDone(travelExpenseReportDoneDto);
        //        }

        //        // Assert
        //        Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
        //        var okObjectResult = actual.Result as OkObjectResult;
        //        Assert.That(okObjectResult, Is.Not.Null);
        //        var value = okObjectResult.Value as TravelExpenseReportDoneResponse;
        //        Assert.That(value, Is.Not.Null);
        //        using (var unitOfWork = testContext.GetUnitOfWork())
        //        {
        //            var travelExpenseEntity = unitOfWork
        //                .Repository
        //                .List(new TravelExpenseById(existingId))
        //                .Single();
        //            Assert.That(travelExpenseEntity.IsReportedDone, Is.EqualTo(true));
        //        }
        //    }
        //}

        //[Test]
        //public void ReportDone_NonExistingTravelExpense_ThrowsTravelExpenseNotFoundByIdException()
        //{
        //    // Arrange
        //    using (var testContext = new IntegrationTestContext())
        //    {
        //        var existingId = Guid.NewGuid();
        //        var travelExpenseReportDoneDto = new TravelExpenseReportDoneDto {Id = existingId};
        //        using (var unitOfWork = testContext.ServiceProvider.GetService<IUnitOfWork>())
        //        {
        //            var sut = GetSut(testContext, unitOfWork);

        //            // Act
        //            var travelExpenseNotFoundByIdException =
        //                Assert.ThrowsAsync<ItemNotFoundException>(async () =>
        //                    await sut.ReportDone(travelExpenseReportDoneDto));

        //            // Assert
        //            Assert.That(travelExpenseNotFoundByIdException, Is.Not.Null);
        //            Assert.That(travelExpenseNotFoundByIdException.Id, Is.EqualTo(existingId.ToString()));
        //        }
        //    }
        //}

        //[Test]
        //public async Task AssignPayment_ExistingTravelExpense_ReturnsOk()
        //{
        //    // Arrange
        //    using (var testContext = new IntegrationTestContext())
        //    {
        //        ActionResult<TravelExpenseAssignPaymentResponse> actual;
        //        Guid existingId;
        //        using (var unitOfWork = testContext.GetUnitOfWork())
        //        {
        //            var existing = unitOfWork.Repository.List<TravelExpenseEntity>().First();
        //            //existing.ReportDone();
        //            //existing.Certify();

        //            existingId = existing.Id;

        //            unitOfWork.Commit();
        //        }

        //        using (var unitOfWork = testContext.GetUnitOfWork())
        //        {
        //            var travelExpenseAssignPaymentDto = new TravelExpenseAssignPaymentDto {Id = existingId};
        //            var sut = GetSut(testContext, unitOfWork);

        //            // Act
        //            actual = await sut.AssignPayment(travelExpenseAssignPaymentDto);
        //        }

        //        // Assert
        //        Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
        //        var okObjectResult = actual.Result as OkObjectResult;
        //        Assert.That(okObjectResult, Is.Not.Null);
        //        var value = okObjectResult.Value as TravelExpenseAssignPaymentResponse;
        //        Assert.That(value, Is.Not.Null);
        //        using (var unitOfWork = testContext.GetUnitOfWork())
        //        {
        //            var travelExpenseEntity = unitOfWork
        //                .Repository
        //                .List(new TravelExpenseById(existingId))
        //                .Single();
        //            Assert.That(travelExpenseEntity.IsAssignedPayment, Is.EqualTo(true));
        //        }
        //    }
        //}

        //[Test]
        //public void AssignPayment_NonExistingTravelExpense_ThrowsTravelExpenseNotFoundByIdException()
        //{
        //    // Arrange
        //    using (var testContext = new IntegrationTestContext())
        //    {
        //        var existingId = Guid.NewGuid();
        //        using (var unitOfWork = testContext.GetUnitOfWork())
        //        {
        //            var travelExpenseReportDoneDto = new TravelExpenseAssignPaymentDto {Id = existingId};
        //            var sut = GetSut(testContext, unitOfWork);

        //            // Act
        //            Assert.ThrowsAsync<ItemNotFoundException>(() => sut.AssignPayment(travelExpenseReportDoneDto));
        //        }
        //    }
        //}

        [Test]
        public async Task Post_ValidNewTravelExpense_ReturnsId()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                _subManagementService.Setup(x => x.GetPolApiContext(It.IsAny<HttpContext>()))
                    .ReturnsAsync(testContext.GetPolApiContext(TestData.DummyPolSubAlice));

                ActionResult<TravelExpenseCreateResponse> actual;
                var newDescription = testContext.Fixture.Create<string>();

                var customerId = testContext
                    .GetUnitOfWork()
                    .Repository
                    .List(new CustomerByName(TestData.DummyCustomerName1))
                    .Single()
                    .Id;
                var travelExpenseCreateDto = TestDataHelper.GetValidTravelExpenseCreateDto(newDescription, customerId);
                var sut = GetSut(testContext);

                // Act
                actual = await sut.Post(travelExpenseCreateDto);

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(CreatedResult)));
                var createdResult = actual.Result as CreatedResult;
                Assert.That(createdResult, Is.Not.Null);
                var value = createdResult.Value as TravelExpenseCreateResponse;
                Assert.That(value, Is.Not.Null);
                Assert.That(value.Id, Is.Not.EqualTo(Guid.Empty));

                using (var unitOfWork = testContext.GetUnitOfWork())
                {
                    var travelExpenseEntity = unitOfWork
                        .Repository.List(new TravelExpenseById(value.Id))
                        .SingleOrDefault();
                    Assert.That(travelExpenseEntity, Is.Not.Null);
                    Assert.That(travelExpenseEntity.Stage.Value, Is.EqualTo(TravelExpenseStage.Initial));
                }
            }
        }

        [Test]
        public async Task Process_ValidTravelExpenseAndStep_ReturnsOk()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                _subManagementService.Setup(x => x.GetPolApiContext(It.IsAny<HttpContext>()))
                    .ReturnsAsync(testContext.GetPolApiContext(TestData.DummyPolSubAlice));

                var sut = GetSut(testContext);

                // Act
                var actual = await sut.Process(testContext.TravelExpenseEntity1.Id,
                    testContext.GetUnitOfWork().Repository
                        .List(new FlowStepByCustomerAndStage(testContext.GetDummyCustomer1Id(),
                            testContext.TravelExpenseEntity1.Stage.Value)).Single().Id); // Globals.InitialReporteddone);

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var createdResult = actual.Result as OkObjectResult;
                Assert.That(createdResult, Is.Not.Null);
                var value = createdResult.Value as TravelExpenseProcessStepResponse;
                Assert.That(value, Is.Not.Null);
            }
        }

        private static TravelExpenseController GetSut(IntegrationTestContext testContext)
        {
            return new TravelExpenseController(testContext.ServiceProvider.GetService<IFlowStepTravelExpenseService>(),
                testContext.ServiceProvider.GetService< IGetTravelExpenseService> (),
                testContext.ServiceProvider.GetService<IUpdateTravelExpenseService>(),
                testContext.ServiceProvider.GetService<ICreateTravelExpenseService>(),
                _subManagementService.Object);
        }
    }
}