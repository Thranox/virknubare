using System;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;
using Web.Controllers;

namespace Tests.TestHelpers
{
    public class IntegrationTestContext :BaseTestContext
    {
        public TravelExpenseEntity TravelExpenseEntity1 = new TravelExpenseEntity("Expense1") {Id = Guid.NewGuid()};
        public TravelExpenseEntity TravelExpenseEntity2 = new TravelExpenseEntity("Expense2") {Id = Guid.NewGuid()};
        public TravelExpenseEntity TravelExpenseEntity3 = new TravelExpenseEntity("Expense3") {Id = Guid.NewGuid()};

        public IntegrationTestContext()
        {
            DbContextOptions = new DbContextOptionsBuilder<PolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            Mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new TravelExpenseProfile())));

            SeedDb();
        }

        public DbContextOptions<PolDbContext> DbContextOptions { get; }
        public IMapper Mapper { get; }

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