using System;
using System.Linq;
using System.Reflection;
using Domain;
using Domain.Interfaces;
using Domain.Specifications;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Tests.TestHelpers;
using Web;

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
                    var repository=new EfRepository(polDbContext);
                    var customer = repository
                        .List(new CustomerByName(TestData.DummyCustomerName))
                        .Single();

                    var newTe = polDbContext.TravelExpenses.First();

                    // Act & Assert
                    do
                    {
                        Console.WriteLine("Stage: " + newTe.Stage);
                        var nextFlowSteps = customer.FlowSteps.First(x => x.From == newTe.Stage);

                        var processFlowStep = processFlowSteps
                            .SingleOrDefault(x => x.CanHandle(nextFlowSteps.Key));
                        Assert.That(processFlowStep, Is.Not.Null);
                        newTe.ApplyProcessStep( processFlowStep);
                    } while (newTe.Stage != TravelExpenseStage.Final);
                }
            }
        }
    }
}