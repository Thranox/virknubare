using System;
using System.Linq;
using System.Reflection;
using Domain;
using Domain.Interfaces;
using Domain.Specifications;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SharedWouldBeNugets;
using Tests.TestHelpers;

namespace Tests.Flows
{
    public class FlowTests
    {
        [Test]
        [Timeout(60000)]
        public void Test()
        {
            // Arrange
            using (var testContext = new IntegrationTestContext())
            {
                var buildServiceProvider = new ServiceCollection();

                Assembly
                    .GetAssembly(typeof(IProcessFlowStep))
                    .GetTypesAssignableFrom<IProcessFlowStep>()
                    .ForEach(t => { buildServiceProvider.AddScoped(typeof(IProcessFlowStep), t); });

                var serviceProvider = buildServiceProvider.BuildServiceProvider();
                var processFlowSteps = serviceProvider
                    .GetServices<IProcessFlowStep>();

                using (var polDbContext = new PolDbContext(testContext.DbContextOptions))
                {
                    var repository = new EfRepository(polDbContext);
                    var customer = repository
                        .List(new CustomerByName(TestData.DummyCustomerName))
                        .Single();

                    var newTe = customer.TravelExpenses.First();

                    var iterations = 0;

                    // Act & Assert
                    do
                    {
                        iterations++;
                        Console.WriteLine("Stage: " + newTe.Stage);

                        var nextFlowSteps = testContext.CreateUnitOfWork().Repository
                            .List(new FlowStepByCustomer(newTe.Stage, customer.Id))
                            .First();

                        var processFlowStep = processFlowSteps
                            .SingleOrDefault(x => x.CanHandle(nextFlowSteps.Key));

                        Assert.That(processFlowStep, Is.Not.Null);

                        newTe.ApplyProcessStep(processFlowStep);

                        if (iterations > 10)
                            throw new InvalidOperationException();
                    } while (newTe.Stage != TravelExpenseStage.Final);
                }
            }
        }
    }
}