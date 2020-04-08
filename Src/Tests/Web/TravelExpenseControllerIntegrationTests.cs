using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Domain.Entities;
using Domain.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
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
                        { Id = existingId, Description = newDescription };
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
        public async Task Put_NonExistingTravelExpenseWithChanges_ReturnsBadRequest()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                ActionResult<TravelExpenseUpdateResponse> actual;
                var newDescription = testContext.Fixture.Create<string>();
                Guid existingId;
                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    existingId = Guid.NewGuid();
                    var travelExpenseUpdateDto = new TravelExpenseUpdateDto
                        { Id = existingId, Description = newDescription };
                    var sut = GetSut(testContext, unitOfWork);

                    // Act
                    actual = await sut.Put(travelExpenseUpdateDto);
                }

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(BadRequestObjectResult)));
                var badRequestObjectResult = actual.Result as BadRequestObjectResult;
                Assert.That(badRequestObjectResult, Is.Not.Null);
                var value = badRequestObjectResult.Value as string;
                Assert.That(value, Is.Not.Null);
            }
        }

        [Test]
        public async Task Approve_ExistingTravelExpense_ReturnsOk()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                ActionResult<TravelExpenseCertifyResponse> actual;
                Guid existingId;
                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var existing = unitOfWork.Repository.List<TravelExpenseEntity>().First();
                    existingId = existing.Id;
                    var travelExpenseApproveDto = new TravelExpenseCertifyDto { Id = existingId };
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
        public async Task Certify_NonExistingTravelExpense_ReturnsBadRequest()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                ActionResult<TravelExpenseCertifyResponse> actual;
                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var existingId = Guid.NewGuid();
                    var travelExpenseCertifyDto = new TravelExpenseCertifyDto { Id = existingId };
                    var sut = GetSut(testContext, unitOfWork);

                    // Act
                    actual = await sut.Certify(travelExpenseCertifyDto);
                }

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(BadRequestObjectResult)));
                var okObjectResult = actual.Result as BadRequestObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as string;
                Assert.That(value, Is.Not.Null);
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
        public async Task ReportDone_NonExistingTravelExpense_ReturnsBadRequest()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                ActionResult<TravelExpenseReportDoneResponse> actual;
                using (var unitOfWork = testContext.CreateUnitOfWork())
                {
                    var existingId = Guid.NewGuid();
                    var travelExpenseReportDoneDto = new TravelExpenseReportDoneDto { Id = existingId };
                    var sut = GetSut(testContext, unitOfWork);

                    // Act
                    actual = await sut.ReportDone(travelExpenseReportDoneDto);
                }

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(BadRequestObjectResult)));
                var okObjectResult = actual.Result as BadRequestObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as string;
                Assert.That(value, Is.Not.Null);
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
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as TravelExpenseCreateResponse;
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
            return new TravelExpenseController(testContext.Logger, testContext.Mapper, unitOfWork, testContext.TravelExpenseValidator);
        }
    }
}