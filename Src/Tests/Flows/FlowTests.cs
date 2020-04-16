using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Application.Interfaces;
using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Tests.TestHelpers;
using Web;

namespace Tests
{
    public class FlowTests
    {
        [Test]
        [Timeout(2000)]
        public void Test()
        {
            using (var testContext = new IntegrationTestContext())
            {

                // A flow consists of flowsteps.
                var buildServiceProvider = new ServiceCollection();

                Assembly
                    .GetAssembly(typeof(IProcessFlowStep))
                    .GetTypesAssignableFrom<IProcessFlowStep>()
                    .ForEach(t => { buildServiceProvider.AddScoped(typeof(IProcessFlowStep), t); });

                var serviceProvider = buildServiceProvider.BuildServiceProvider();
                var processFlowSteps = serviceProvider
                    .GetServices<IProcessFlowStep>();
                Assert.That(processFlowSteps.Any(), Is.True);

                // Arrange
                var customer = new CustomerEntity("dummy");
                customer.Steps.Add(new FlowStepEntity(Globals.InitialReporteddone, TravelExpenseStage.Initial));
                customer.Steps.Add(new FlowStepEntity(Globals.ReporteddoneCertified, TravelExpenseStage.ReportedDone));
                customer.Steps.Add(
                    new FlowStepEntity(Globals.CertifiedAssignedForPayment, TravelExpenseStage.Certified));
                customer.Steps.Add(new FlowStepEntity(Globals.AssignedForPaymentFinal,
                    TravelExpenseStage.AssignedForPayment));

                var newTe = new TravelExpenseEntity("dummy");

                do
                {
                    Console.WriteLine("Stage: " + newTe.Stage);
                    var nextFlowSteps = customer.Steps.First(x => x.From == newTe.Stage);

                    var processFlowStep = processFlowSteps
                        .SingleOrDefault(x => x.CanHandle(nextFlowSteps.Key));
                    processFlowStep.Process(newTe);
                } while (newTe.Stage != TravelExpenseStage.Final);
            }
        }
    }
}