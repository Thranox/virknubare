using System;
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

            SeedDb();
        }

        public Logger Logger { get; set; }

        private void SeedDb()
        {
            using (var dbContext = new PolDbContext(DbContextOptions))
            {
                dbContext.TravelExpenses.Add(new TravelExpenseEntity() { Description = "Expense1" });
                dbContext.TravelExpenses.Add(new TravelExpenseEntity() { Description = "Expense2" });
                dbContext.TravelExpenses.Add(new TravelExpenseEntity() { Description = "Expense3" });

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