using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Domain;
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
            using (var context = new PolDbContext(testContext.DbContextOptions))
            {
                var efRepository = new EfRepository(context);
                var sut = new TravelExpenseController(testContext.Logger, efRepository, testContext.Mapper);

                // Act
                var actual = await sut.Get();

                // Assert
                var travelExpenseDtos = actual.ToArray();
                Assert.That(travelExpenseDtos.Length, Is.EqualTo(3));
                Assert.That(travelExpenseDtos,
                    Has.One.EqualTo(new TravelExpenseDto
                    {
                        Description = testContext.TravelExpenseEntity1.Description,
                        PublicId = testContext.TravelExpenseEntity1.PublicId
                    }));
                Assert.That(travelExpenseDtos,
                    Has.One.EqualTo(new TravelExpenseDto
                    {
                        Description = testContext.TravelExpenseEntity2.Description,
                        PublicId = testContext.TravelExpenseEntity2.PublicId
                    }));
                Assert.That(travelExpenseDtos,
                    Has.One.EqualTo(new TravelExpenseDto
                    {
                        Description = testContext.TravelExpenseEntity3.Description,
                        PublicId = testContext.TravelExpenseEntity3.PublicId
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
                Guid existingPublicId;
                using (var context = new PolDbContext(testContext.DbContextOptions))
                {
                    var efRepository = new EfRepository(context);
                    var existing = efRepository.List<TravelExpenseEntity>().First();
                    existingPublicId = existing.PublicId;
                    var travelExpenseUpdateDto = new TravelExpenseUpdateDto
                        {PublicId = existingPublicId, Description = newDescription};
                    var sut = new TravelExpenseController(testContext.Logger, efRepository, testContext.Mapper);

                    // Act
                    actual = await sut.Put(travelExpenseUpdateDto);
                }

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as TravelExpenseUpdateResponse;
                Assert.That(value, Is.Not.Null);
                Assert.That(value.Result, Is.EqualTo(true));
                using (var context = new PolDbContext(testContext.DbContextOptions))
                {
                    var efRepository = new EfRepository(context);
                    var travelExpenseEntity = efRepository
                        .List(new TravelExpenseByPublicId(existingPublicId))
                        .SingleOrDefault();
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
                Guid existingPublicId;
                using (var context = new PolDbContext(testContext.DbContextOptions))
                {
                    var efRepository = new EfRepository(context);
                    var existing = efRepository.List<TravelExpenseEntity>().First();
                    existingPublicId = existing.PublicId;
                    var travelExpenseApproveDto = new TravelExpenseApproveDto { PublicId = existingPublicId };
                    var sut = new TravelExpenseController(testContext.Logger, efRepository, testContext.Mapper);

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
                        .List(new TravelExpenseByPublicId(existingPublicId))
                        .SingleOrDefault();
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
                Guid existingPublicId;
                using (var context = new PolDbContext(testContext.DbContextOptions))
                {
                    var efRepository = new EfRepository(context);
                    var existing = efRepository.List<TravelExpenseEntity>().First();
                    existingPublicId = existing.PublicId;
                    var travelExpenseReportDoneDto = new TravelExpenseReportDoneDto { PublicId = existingPublicId };
                    var sut = new TravelExpenseController(testContext.Logger, efRepository, testContext.Mapper);

                    // Act
                    actual = await sut.ReportDone(travelExpenseReportDoneDto);
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
                        .List(new TravelExpenseByPublicId(existingPublicId))
                        .SingleOrDefault();
                    Assert.That(travelExpenseEntity.IsReportedDone, Is.EqualTo(true));
                }
            }
        }

        [Test]
        public async Task Post_ValidNewTravelExpense_ReturnsPublicId()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                ActionResult<TravelExpenseCreateResponse> actual;
                var newDescription = testContext.Fixture.Create<string>();
                using (var context = new PolDbContext(testContext.DbContextOptions))
                {
                    var efRepository = new EfRepository(context);
                    var travelExpenseCreateDto = new TravelExpenseCreateDto {Description = newDescription};
                    var sut = new TravelExpenseController(testContext.Logger, efRepository, testContext.Mapper);

                    // Act
                    actual = await sut.Post(travelExpenseCreateDto);
                }

                // Assert
                Assert.That(actual.Result, Is.InstanceOf(typeof(OkObjectResult)));
                var okObjectResult = actual.Result as OkObjectResult;
                Assert.That(okObjectResult, Is.Not.Null);
                var value = okObjectResult.Value as TravelExpenseCreateResponse;
                Assert.That(value, Is.Not.Null);
                Assert.That(value.PublicId, Is.Not.EqualTo(Guid.Empty));
                using (var context = new PolDbContext(testContext.DbContextOptions))
                {
                    var efRepository = new EfRepository(context);
                    var travelExpenseEntity = efRepository.List(new TravelExpenseByPublicId(value.PublicId))
                        .SingleOrDefault();
                    Assert.That(travelExpenseEntity, Is.Not.Null);
                    Assert.That(travelExpenseEntity.IsReportedDone, Is.EqualTo(false));
                    Assert.That(travelExpenseEntity.IsApproved, Is.EqualTo(false));
                }
            }
        }
    }

}