using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Shared;
using API.Shared.Controllers;
using API.Shared.Services;
using Application;
using Application.MapperProfiles;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SharedWouldBeNugets;

namespace Tests.TestHelpers
{
    public class IntegrationTestContext : BaseTestContext
    {
        private PolDbContext _polDbContext;
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
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json", false, true);

            ServicesConfiguration.MapServices(serviceCollection, false, configurationBuilder.Build());

            serviceCollection.AddScoped(x => Log.Logger);
            serviceCollection.AddScoped(x => GetUnitOfWork());
            serviceCollection.AddTransient(x => GetPolDbContext(x));
            serviceCollection.AddScoped<TravelExpenseController>();
            serviceCollection.AddScoped<FlowStepController>();
            serviceCollection.AddScoped<IMessageSenderService, MemoryListMessageSenderService>();

            ServiceProvider = serviceCollection.BuildServiceProvider();

            SeedDb().Wait();
        }

        public DbContextOptions<PolDbContext> DbContextOptions { get; }
        public IMapper Mapper { get; }
        public IServiceProvider ServiceProvider { get; set; }

        private PolDbContext GetPolDbContext(IServiceProvider x)
        {
            if (_polDbContext == null)
                _polDbContext = new PolDbContext(DbContextOptions, x.GetRequiredService<IDomainEventDispatcher>());
            return _polDbContext;
        }

        private async Task SeedDb()
        {
            var dbSeeder = ServiceProvider.GetService<IDbSeeder>();
            await dbSeeder.SeedAsync();

            using (var unitOfWork = ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>())
            {
                var customer = unitOfWork
                    .Repository
                    .List(new CustomerByName(TestData.DummyCustomerName1))
                    .SingleOrDefault();

                var travelExpenseEntities = customer.TravelExpenses.ToList();
                TravelExpenseEntity1 = travelExpenseEntities[0];
                TravelExpenseEntity2 = travelExpenseEntities[1];
                TravelExpenseEntity3 = travelExpenseEntities[2];

                await unitOfWork.CommitAsync();
            }
        }

        public IUnitOfWork GetUnitOfWork()
        {
            var context = ServiceProvider.GetRequiredService<PolDbContext>();
            return new UnitOfWork(new EfRepository(context));
        }

        public Guid GetDummyCustomer1Id()
        {
            return GetDummyCustomer1().Id;
        }

        public CustomerEntity GetDummyCustomer1()
        {
            return GetUnitOfWork()
                .Repository.List(new CustomerByName(TestData.DummyCustomerName1))
                .Single();
        }
        public Guid GetDummyCustomer2Id()
        {
            return GetDummyCustomer2().Id;
        }

        public CustomerEntity GetDummyCustomer2()
        {
            return GetUnitOfWork()
                .Repository.List(new CustomerByName(TestData.DummyCustomerName2))
                .Single();
        }

        public List<StageEntity> GetStages()
        {
            return GetUnitOfWork()
                .Repository
                .List<StageEntity>();
        }

        public void SetCallingUserBySub(string sub)
        {
            (ServiceProvider.GetRequiredService<ISubManagementService>() as FakeSubManagementService)
                .PolApiContext = GetPolApiContext(sub);
        }

        public PolApiContext GetPolApiContext(string sub)
        {
            return new PolApiContext(
                GetUnitOfWork()
                    .Repository
                    .List(new UserBySub(sub)).Single(),
                "http://nowhere.com",
                new PolSystem("http://nowhere.com/api", "http://nowhere.com/web")
            );
        }
    }
}