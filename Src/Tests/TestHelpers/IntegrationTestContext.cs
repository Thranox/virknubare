using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API.Shared;
using API.Shared.Controllers;
using Application.MapperProfiles;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Services;
using Domain.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SharedWouldBeNugets;

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
            var configurationBuilder = new ConfigurationBuilder();

            ServicesConfiguration.MapServices(serviceCollection, false, configurationBuilder.Build());
            
            serviceCollection.AddScoped<ILogger>(x => Logger);
            serviceCollection.AddScoped(x => CreateUnitOfWork());
            serviceCollection.AddScoped<TravelExpenseController>();
            serviceCollection.AddScoped<FlowStepController>();
            serviceCollection.AddScoped<IMessageSenderService, MemoryListMessageSenderService>();


            ServiceProvider = serviceCollection.BuildServiceProvider();

            SeedDb().Wait();
        }

        public DbContextOptions<PolDbContext> DbContextOptions { get; }
        public IMapper Mapper { get; }
        public IServiceProvider ServiceProvider { get; set; }

        private async Task SeedDb()
        {
            var dbSeeder = ServiceProvider.GetService<IDbSeeder>();
            await dbSeeder.SeedAsync();

            using (var unitOfWork=ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>())
            {
                var customer = unitOfWork
                    .Repository
                    .List(new CustomerByName(TestData.DummyCustomerName))
                    .SingleOrDefault();

                var travelExpenseEntities = customer.TravelExpenses.ToList();
                TravelExpenseEntity1 = travelExpenseEntities[0];
                TravelExpenseEntity2 = travelExpenseEntities[1];
                TravelExpenseEntity3 = travelExpenseEntities[2];

                await unitOfWork.CommitAsync();
            }
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            var domainEventDispatcher = ServiceProvider.GetRequiredService<IDomainEventDispatcher>();
            var context = new PolDbContext(DbContextOptions, domainEventDispatcher);
            return new UnitOfWork(new EfRepository(context));
        }

        public Guid GetDummyCustomerId()
        {
            return GetDummyCustomer().Id;
        }

        public CustomerEntity GetDummyCustomer()
        {
            return CreateUnitOfWork().
                Repository.
                List(new CustomerByName(TestData.DummyCustomerName)).
                Single();
        }

        public List<StageEntity> GetStages()
        {
            return CreateUnitOfWork().
                Repository.
                List<StageEntity>();
        }
    }
}