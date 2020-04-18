using System;
using System.Linq;
using System.Reflection;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Web;

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
            //Mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new TravelExpenseProfile())));

            var buildServiceProvider = new ServiceCollection();
            buildServiceProvider.AddScoped<IGetTravelExpenseService, GetTravelExpenseService>();
            buildServiceProvider.AddScoped<ICreateTravelExpenseService, CreateTravelExpenseService>();
            buildServiceProvider.AddScoped<IUpdateTravelExpenseService, UpdateTravelExpenseService>();
            buildServiceProvider.AddScoped<IProcessStepTravelExpenseService, ProcessStepTravelExpenseService>();
            
            buildServiceProvider.AddScoped(x => Serilog.Log.Logger);
            buildServiceProvider.AddScoped(x => CreateUnitOfWork());
            buildServiceProvider.AddAutoMapper(typeof(Startup));

            Assembly
                .GetAssembly(typeof(IProcessFlowStep))
                .GetTypesAssignableFrom<IProcessFlowStep>()
                .ForEach(t => { buildServiceProvider.AddScoped(typeof(IProcessFlowStep), t); });

            ServiceProvider = buildServiceProvider.BuildServiceProvider();

            SeedDb();
        }

        public DbContextOptions<PolDbContext> DbContextOptions { get; }
        //public IMapper Mapper { get; }
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