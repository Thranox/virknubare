using System;
using System.Linq;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Web;
using Web.MapperProfiles;

namespace Tests.TestHelpers
{
    public class IntegrationTestContext :BaseTestContext
    {
        public TravelExpenseEntity TravelExpenseEntity1;
        public TravelExpenseEntity TravelExpenseEntity2;
        public TravelExpenseEntity TravelExpenseEntity3;

        public IntegrationTestContext()
        {
            DbContextOptions = new DbContextOptionsBuilder<PolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            Mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new EntityDtoProfile())));

            var serviceCollection = new ServiceCollection();
            Startup.AddToServiceCollection(serviceCollection);

            serviceCollection.AddScoped(x => Serilog.Log.Logger);
            serviceCollection.AddScoped(x => CreateUnitOfWork());

            ServiceProvider = serviceCollection.BuildServiceProvider();

            SeedDb();
        }

        public DbContextOptions<PolDbContext> DbContextOptions { get; }
        public IMapper Mapper { get; }
        public IServiceProvider ServiceProvider { get; set; }

        private void SeedDb()
        {
            using (var dbContext = new PolDbContext(DbContextOptions))
            {
                dbContext.Seed();
                var travelExpenseEntities = dbContext.CustomerEntities.First().TravelExpenses.ToList();
                TravelExpenseEntity1 = travelExpenseEntities[0];
                TravelExpenseEntity2 = travelExpenseEntities[1];
                TravelExpenseEntity3 = travelExpenseEntities[2];

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