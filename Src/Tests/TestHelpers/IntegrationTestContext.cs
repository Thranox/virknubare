using System;
using AutoFixture;
using AutoMapper;
using Domain;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;
using Web.Controllers;
using Web.Validation;
using Web.Validation.ValidationRules;

namespace Tests
{
    public class IntegrationTestContext : IDisposable
    {
        public TravelExpenseEntity TravelExpenseEntity1 = new TravelExpenseEntity("Expense1") {Id = Guid.NewGuid()};
        public TravelExpenseEntity TravelExpenseEntity2 = new TravelExpenseEntity("Expense2") {Id = Guid.NewGuid()};
        public TravelExpenseEntity TravelExpenseEntity3 = new TravelExpenseEntity("Expense3") {Id = Guid.NewGuid()};

        public IntegrationTestContext()
        {
            Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            Logger.Information("Starting test");
            DbContextOptions = new DbContextOptionsBuilder<PolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            Mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new TravelExpenseProfile())));
            TravelExpenseValidator = new TravelExpenseValidator(new ITravelExpenseValidatorRule[]{new TravelExpenseValidatorRule_ChangeToExisting_MustExist()});

            Fixture = new Fixture();
            SeedDb();
        }

        public Fixture Fixture { get; }
        public Logger Logger { get; set; }

        public DbContextOptions<PolDbContext> DbContextOptions { get; }
        public IMapper Mapper { get; }
        public ITravelExpenseValidator TravelExpenseValidator
        { get; }

        public void Dispose()
        {
            Logger.Information("Ending test");
        }

        private void SeedDb()
        {
            using (var dbContext = new PolDbContext(DbContextOptions))
            {
                dbContext.TravelExpenses.Add(TravelExpenseEntity1);
                dbContext.TravelExpenses.Add(TravelExpenseEntity2);
                dbContext.TravelExpenses.Add(TravelExpenseEntity3);

                dbContext.SaveChanges();
            }
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            var context = new PolDbContext(DbContextOptions);
            return new UnitOfWork(new EfRepository(context));
        }
    }
}