using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using AutoFixture;
using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Tests.TestHelpers;
using Web.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Web
{
    public class TravelExpenseControllerIntegrationTests
    {
        [Test]
        public async Task Get_NoParameters_ReturnsTravelExpenses()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var sut = GetSut(testContext, unitOfWork);

                    // Act
                    var actual = await sut.Get();

                    // Assert
                    Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                    var okObjectResult = actual.Result as OkObjectResult;
                    Assert.That(okObjectResult, Is.Not.Null);
                    var value = okObjectResult.Value as TravelExpenseGetResponse;
                    Assert.That(value, Is.Not.Null);
                    var v = value.Result.ToArray();
                    Assert.That(v.Length, Is.EqualTo(3));
                    Assert.That(v,
                        Has.One.EqualTo(new TravelExpenseDto
                        {
                            Description = testContext.TravelExpenseEntity1.Description,
                            Id = testContext.TravelExpenseEntity1.Id
                        }));
                    Assert.That(v,
                        Has.One.EqualTo(new TravelExpenseDto
                        {
                            Description = testContext.TravelExpenseEntity2.Description,
                            Id = testContext.TravelExpenseEntity2.Id
                        }));
                    Assert.That(v,
                        Has.One.EqualTo(new TravelExpenseDto
                        {
                            Description = testContext.TravelExpenseEntity3.Description,
                            Id = testContext.TravelExpenseEntity3.Id
                        }));
                }
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
                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var existing = unitOfWork.Repository.List<TravelExpenseEntity>().First();
                    existingId = existing.Id;
                    var travelExpenseUpdateDto = new TravelExpenseUpdateDto
                        {Id = existingId, Description = newDescription};
                    var sut = GetSut(testContext, unitOfWork);

                    // Act
                    actual = await sut.Put(travelExpenseUpdateDto);
                }

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as TravelExpenseUpdateResponse;
                Assert.That(value, Is.Not.Null);
                using (var unitOfWork = testContext.CreateUnitOfWork())
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
                var travelExpenseUpdateDto = new TravelExpenseUpdateDto { Id = existingId, Description = newDescription };

                using (var unitOfWork = testContext.ServiceProvider.GetService<IUnitOfWork>())
                {
                    var sut = GetSut(testContext, unitOfWork);

                    // Act
                    var travelExpenseNotFoundByIdException = Assert.ThrowsAsync<TravelExpenseNotFoundByIdException>(async () => await sut.Put(travelExpenseUpdateDto));

                    // Assert
                    Assert.That(travelExpenseNotFoundByIdException, Is.Not.Null);
                    Assert.That(travelExpenseNotFoundByIdException.Id, Is.EqualTo(existingId));
                }
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
                    existing.ReportDone();
                    var travelExpenseUpdateDto = new TravelExpenseUpdateDto { Id = existing.Id, Description = newDescription };
                    var sut = GetSut(testContext, unitOfWork);

                    // Act
                    var businessRuleViolationException = Assert.ThrowsAsync<BusinessRuleViolationException>(async ()=> await sut.Put(travelExpenseUpdateDto));

                    // Assert
                    Assert.That(businessRuleViolationException, Is.Not.Null);
                    Assert.That(businessRuleViolationException.EntityId, Is.EqualTo(existing.Id));
                }
            }
        }

        [Test]
        public async Task Certify_ExistingTravelExpense_ReturnsOk()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                ActionResult<TravelExpenseCertifyResponse> actual;
                Guid existingId;
                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var existing = unitOfWork.Repository.List<TravelExpenseEntity>().First();
                    existing.ReportDone();
                    existingId = existing.Id;
                    unitOfWork.Commit();
                }

                using (var unitOfWork = testContext.CreateUnitOfWork())
                { 
                    var travelExpenseApproveDto = new TravelExpenseCertifyDto {Id = existingId};
                    var sut = GetSut(testContext, unitOfWork);

                    // Act
                    actual = await sut.Certify(travelExpenseApproveDto);
                }

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as TravelExpenseCertifyResponse;
                Assert.That(value, Is.Not.Null);
                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var travelExpenseEntity = unitOfWork
                        .Repository
                        .List(new TravelExpenseById(existingId))
                        .Single();
                    Assert.That(travelExpenseEntity.IsCertified, Is.EqualTo(true));
                }
            }
        }

        [Test]
        public void Certify_NonExistingTravelExpense_ThrowsTravelExpenseNotFoundByIdException()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var existingId = Guid.NewGuid();
                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var travelExpenseCertifyDto = new TravelExpenseCertifyDto {Id = existingId};
                    var sut = GetSut(testContext, unitOfWork);

                    // Act
                    Assert.ThrowsAsync<TravelExpenseNotFoundByIdException>( ()=>sut.Certify(travelExpenseCertifyDto));
                }
            }
        }

        [Test]
        public async Task ReportDone_ExistingTravelExpense_ReturnsOk()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                ActionResult<TravelExpenseReportDoneResponse> actual;
                Guid existingId;
                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var existing = unitOfWork.Repository.List<TravelExpenseEntity>().First();
                    existingId = existing.Id;
                    var travelExpenseReportDoneDto = new TravelExpenseReportDoneDto { Id = existingId };
                    var sut = GetSut(testContext, unitOfWork);

                    // Act
                    actual = await sut.ReportDone(travelExpenseReportDoneDto);
                }

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as TravelExpenseReportDoneResponse;
                Assert.That(value, Is.Not.Null);
                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var travelExpenseEntity = unitOfWork
                        .Repository
                        .List(new TravelExpenseById(existingId))
                        .Single();
                    Assert.That(travelExpenseEntity.IsReportedDone, Is.EqualTo(true));
                }
            }
        }

        [Test]
        public void ReportDone_NonExistingTravelExpense_ThrowsTravelExpenseNotFoundByIdException()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var existingId = Guid.NewGuid();
                var travelExpenseReportDoneDto = new TravelExpenseReportDoneDto { Id = existingId };
                using (var unitOfWork = testContext.ServiceProvider.GetService<IUnitOfWork>())
                {
                    var sut = GetSut(testContext, unitOfWork);

                    // Act
                    var travelExpenseNotFoundByIdException = Assert.ThrowsAsync<TravelExpenseNotFoundByIdException>(async () => await sut.ReportDone(travelExpenseReportDoneDto));

                    // Assert
                    Assert.That(travelExpenseNotFoundByIdException, Is.Not.Null);
                    Assert.That(travelExpenseNotFoundByIdException.Id, Is.EqualTo(existingId));
                }
            }
        }

        [Test]
        public async Task AssignPayment_ExistingTravelExpense_ReturnsOk()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                ActionResult<TravelExpenseAssignPaymentResponse> actual;
                Guid existingId;
                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var existing = unitOfWork.Repository.List<TravelExpenseEntity>().First();
                    existing.ReportDone();
                    existing.Certify();
                    
                    existingId = existing.Id;

                    unitOfWork.Commit();
                }

                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var travelExpenseAssignPaymentDto = new TravelExpenseAssignPaymentDto { Id = existingId };
                    var sut = GetSut(testContext, unitOfWork);

                    // Act
                    actual = await sut.AssignPayment(travelExpenseAssignPaymentDto);
                }

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as TravelExpenseAssignPaymentResponse;
                Assert.That(value, Is.Not.Null);
                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var travelExpenseEntity = unitOfWork
                        .Repository
                        .List(new TravelExpenseById(existingId))
                        .Single();
                    Assert.That(travelExpenseEntity.IsAssignedPayment, Is.EqualTo(true));
                }
            }
        }

        [Test]
        public void AssignPayment_NonExistingTravelExpense_ThrowsTravelExpenseNotFoundByIdException()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var existingId = Guid.NewGuid();
                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var travelExpenseReportDoneDto = new TravelExpenseAssignPaymentDto { Id = existingId };
                    var sut = GetSut(testContext, unitOfWork);

                    // Act
                    Assert.ThrowsAsync<TravelExpenseNotFoundByIdException>(()=>sut.AssignPayment(travelExpenseReportDoneDto));
                }
            }
        }

        [Test]
        public async Task Post_ValidNewTravelExpense_ReturnsId()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                ActionResult<TravelExpenseCreateResponse> actual;
                var newDescription = testContext.Fixture.Create<string>();

                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var travelExpenseCreateDto = new TravelExpenseCreateDto {Description = newDescription};
                    var sut = GetSut(testContext, unitOfWork);

                    // Act
                    actual = await sut.Post(travelExpenseCreateDto);
                }

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(CreatedResult)));
                var createdResult = actual.Result as CreatedResult;
                Assert.That(createdResult, Is.Not.Null);
                var value = createdResult.Value as TravelExpenseCreateResponse;
                Assert.That(value, Is.Not.Null);
                Assert.That(value.Id, Is.Not.EqualTo(Guid.Empty));

                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var travelExpenseEntity = unitOfWork
                        .Repository.List(new TravelExpenseById(value.Id))
                        .SingleOrDefault();
                    Assert.That(travelExpenseEntity, Is.Not.Null);
                    Assert.That(travelExpenseEntity.IsReportedDone, Is.EqualTo(false));
                    Assert.That(travelExpenseEntity.IsCertified, Is.EqualTo(false));
                }
            }
        }

        private static TravelExpenseController GetSut(IntegrationTestContext testContext, IUnitOfWork unitOfWork)
        {
            return new TravelExpenseController(testContext.ServiceProvider );
        }
    }
}