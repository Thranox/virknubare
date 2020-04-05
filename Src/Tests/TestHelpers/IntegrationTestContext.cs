using System;
using AutoFixture;
using AutoMapper;
using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;
using Web.Controllers;

namespace Tests
{
    public class IntegrationTestContext : IDisposable
    {
        public TravelExpenseEntity TravelExpenseEntity1 = new TravelExpenseEntity("Expense1");
        public TravelExpenseEntity TravelExpenseEntity2 = new TravelExpenseEntity("Expense2");
        public TravelExpenseEntity TravelExpenseEntity3 = new TravelExpenseEntity( "Expense3");

        public IntegrationTestContext()
        {
            Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            Logger.Information("Starting test");
            DbContextOptions = new DbContextOptionsBuilder<PolDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            Mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new TravelExpenseProfile())));
            Fixture = new Fixture();
            SeedDb();
        }
        public Fixture Fixture { get; }
        public Logger Logger { get; set; }

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

        public DbContextOptions<PolDbContext> DbContextOptions { get; }
        public IMapper Mapper { get; }

        public void Dispose()
        {
            Logger.Information("Ending test");
        }
    }
}