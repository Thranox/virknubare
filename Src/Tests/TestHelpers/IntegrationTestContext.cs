using System;
using AutoMapper;
using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Web.Controllers;

namespace Tests
{
    public class IntegrationTestContext : IDisposable
    {
        public IntegrationTestContext()
        {
            DbContextOptions = new DbContextOptionsBuilder<PolDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            LoggerFactory = new LoggerFactory();
            Mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new TravelExpenseProfile())));

            SeedDb();
        }

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

        public LoggerFactory LoggerFactory { get; }

        public DbContextOptions<PolDbContext> DbContextOptions { get; }
        public IMapper Mapper { get; }

        public void Dispose()
        {
            
        }
    }
}