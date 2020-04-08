using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Domain;
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
                using (var context = new PolDbContext(testContext.DbContextOptions))
                    using(var unitOfWork=new UnitOfWork(new EfRepository(context)))
                {
                    var efRepository = new EfRepository(context);
                    var sut = new TravelExpenseController(testContext.Logger, testContext.Mapper, unitOfWork);

                    // Act
                    var actual = await sut.Get();

                    // Assert
                    Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                    var okObjectResult = actual.Result as OkObjectResult;
                    Assert.That(okObjectResult, Is.Not.Null);
                    var value = okObjectResult.Value as TravelExpenseGetResponse;
                    Assert.That(value, Is.Not.Null);
                    var v=(value.Result as IEnumerable<TravelExpenseDto>).ToArray();
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
                    using (var context = new PolDbContext(testContext.DbContextOptions))
                    using (var unitOfWork = new UnitOfWork(new EfRepository(context)))
                { 
                    var existing =unitOfWork.Repository.List<TravelExpenseEntity>().First();
                    existingId = existing.Id;
                    var travelExpenseUpdateDto = new TravelExpenseUpdateDto
                        {Id = existingId, Description = newDescription};
                    var sut = new TravelExpenseController(testContext.Logger, testContext.Mapper,unitOfWork);

                    // Act
                    actual = await sut.Put(travelExpenseUpdateDto);
                }

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as TravelExpenseUpdateResponse;
                Assert.That(value, Is.Not.Null);
                using (var context = new PolDbContext(testContext.DbContextOptions))
                {
                    var efRepository = new EfRepository(context);
                    var travelExpenseEntity = efRepository
                        .List(new TravelExpenseById(existingId))
                        .Single();
                    Assert.That(travelExpenseEntity.Description, Is.EqualTo(newDescription));
                }
            }
        }

        [Test]
        public async Task Approve_ExistingTravelExpense_ReturnsOk()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                ActionResult<TravelExpenseApproveResponse> actual;
                var newDescription = testContext.Fixture.Create<string>();
                Guid existingId;
                using (var context = new PolDbContext(testContext.DbContextOptions))
                using (var unitOfWork = new UnitOfWork(new EfRepository(context)))
                {
                    var existing = unitOfWork.Repository.List<TravelExpenseEntity>().First();
                    existingId = existing.Id;
                    var travelExpenseApproveDto = new TravelExpenseApproveDto {Id = existingId};
                    var sut = new TravelExpenseController(testContext.Logger, testContext.Mapper,unitOfWork);

                    // Act
                    actual = await sut.Approve(travelExpenseApproveDto);
                }

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as TravelExpenseApproveResponse;
                Assert.That(value, Is.Not.Null);
                using (var context = new PolDbContext(testContext.DbContextOptions))
                {
                    var efRepository = new EfRepository(context);
                    var travelExpenseEntity = efRepository
                        .List(new TravelExpenseById(existingId))
                        .Single();
                    Assert.That(travelExpenseEntity.IsApproved, Is.EqualTo(true));
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
                var newDescription = testContext.Fixture.Create<string>();
                Guid existingId;
                using (var context = new PolDbContext(testContext.DbContextOptions))
                using (var unitOfWork = new UnitOfWork(new EfRepository(context)))
                {
                    var existing = unitOfWork.Repository.List<TravelExpenseEntity>().First();
                    existingId = existing.Id;
                    var travelExpenseReportDoneDto = new TravelExpenseReportDoneDto {Id = existingId};
                    var sut = new TravelExpenseController(testContext.Logger, testContext.Mapper,unitOfWork);

                    // Act
                    actual = await sut.ReportDone(travelExpenseReportDoneDto);
                }

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as TravelExpenseReportDoneResponse;
                Assert.That(value, Is.Not.Null);
                using (var context = new PolDbContext(testContext.DbContextOptions))
                {
                    var efRepository = new EfRepository(context);
                    var travelExpenseEntity = efRepository
                        .List(new TravelExpenseById(existingId))
                        .Single();
                    Assert.That(travelExpenseEntity.IsReportedDone, Is.EqualTo(true));
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
                using (var context = new PolDbContext(testContext.DbContextOptions))
                using (var unitOfWork = new UnitOfWork(new EfRepository(context)))
                {
                    var travelExpenseCreateDto = new TravelExpenseCreateDto {Description = newDescription};
                    var sut = new TravelExpenseController(testContext.Logger, testContext.Mapper,unitOfWork);

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
                using (var context = new PolDbContext(testContext.DbContextOptions))
                {
                    var efRepository = new EfRepository(context);
                    var travelExpenseEntity = efRepository.List(new TravelExpenseById(value.Id))
                        .SingleOrDefault();
                    Assert.That(travelExpenseEntity, Is.Not.Null);
                    Assert.That(travelExpenseEntity.IsReportedDone, Is.EqualTo(false));
                    Assert.That(travelExpenseEntity.IsApproved, Is.EqualTo(false));
                }
            }
        }
    }
}