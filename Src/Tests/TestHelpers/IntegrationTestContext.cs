using System;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Web.Controllers;
using Web;

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

            var buildServiceProvider = new ServiceCollection();
            buildServiceProvider.AddScoped<IAssignPaymentTravelExpenseService, AssignPaymentTravelExpenseService>();
            buildServiceProvider.AddScoped<ICertifyTravelExpenseService, CertifyTravelExpenseService>();
            buildServiceProvider.AddScoped<IGetTravelExpenseService, GetTravelExpenseService>();
            buildServiceProvider.AddScoped<ICreateTravelExpenseService, CreateTravelExpenseService>();
            buildServiceProvider.AddScoped<IUpdateTravelExpenseService, UpdateTravelExpenseService>();
            buildServiceProvider.AddScoped<IReportDoneTravelExpenseService, ReportDoneTravelExpenseService>();
            buildServiceProvider.AddScoped(x => Serilog.Log.Logger);
            buildServiceProvider.AddScoped(x => CreateUnitOfWork());
            buildServiceProvider.AddAutoMapper(typeof(Startup));

            ServiceProvider = buildServiceProvider.BuildServiceProvider();

            SeedDb();
        }

        public DbContextOptions<PolDbContext> DbContextOptions { get; }
        public IMapper Mapper { get; }
        public IServiceProvider ServiceProvider { get; set; }

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