using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Web;

namespace Tests
{
    public class FlowTests
    {
        [Test]
        [Timeout(2000)]
        public void Test()
        {
            // A flow consists of flowsteps.
            var buildServiceProvider = new ServiceCollection();

            Assembly
                .GetAssembly(typeof(FlowTests))
                .GetTypesAssignableFrom<IProcessFlowStep>()
                .ForEach(t => { buildServiceProvider.AddScoped(typeof(IProcessFlowStep), t); });

            var serviceProvider = buildServiceProvider.BuildServiceProvider();
            var processFlowSteps = serviceProvider
                .GetServices<IProcessFlowStep>();

            // Arrange
            var customer = new CustomerEntity("dummy");
            customer.Steps.Add(new FlowStepEntity(Globals.InitialReporteddone,TravelExpenseStage.Initial));
            customer.Steps.Add(new FlowStepEntity(Globals.ReporteddoneCertified, TravelExpenseStage.ReportedDone));
            customer.Steps.Add(new FlowStepEntity(Globals.CertifiedAssignedForPayment, TravelExpenseStage.Certified));
            customer.Steps.Add(new FlowStepEntity(Globals.AssignedForPaymentFinal, TravelExpenseStage.AssignedForPayment));

            var newTe = new TravelExpenseEntity("dummy") ;

            do
            {
                Console.WriteLine("Stage: " + newTe.Stage);
                var nextFlowSteps = customer.Steps.First(x => x.From==newTe.Stage);

                var processFlowStep = processFlowSteps
                    .SingleOrDefault(x => x.CanHandle(nextFlowSteps.Key));
                processFlowStep.Process(newTe);
            } while (newTe.Stage != TravelExpenseStage.Final);
        }
    }

    public interface IProcessFlowStep
    {
        bool CanHandle(string key);
        void Process(TravelExpenseEntity newte);
    }

    public class ProcessFlowStepInitialReportedDone : IProcessFlowStep
    {
        private readonly TravelExpenseStage _fromStage = TravelExpenseStage.Initial;

        public bool CanHandle(string key)
        {
            return key ==Globals.InitialReporteddone;
        }

        public void Process(TravelExpenseEntity newte)
        {
            newte.ReportDone();
        }
    }

    public class ProcessFlowStepReportedDoneCertified : IProcessFlowStep
    {
        private readonly TravelExpenseStage _fromStage = TravelExpenseStage.ReportedDone;

        public bool CanHandle(string key)
        {
            return key == Globals.ReporteddoneCertified;
        }

        public void Process(TravelExpenseEntity newte)
        {
            newte.Certify();
        }
    }

    public class ProcessFlowStepCertifiedAssignedForPayment : IProcessFlowStep
    {
        private readonly TravelExpenseStage _fromStage = TravelExpenseStage.Certified;

        public bool CanHandle(string key)
        {
            return key == Globals.CertifiedAssignedForPayment;
        }

        public void Process(TravelExpenseEntity newte)
        {
            newte.AssignPayment();
        }
    }
    public class ProcessFlowStepAssignedForPaymentFinal : IProcessFlowStep
    {
        private readonly TravelExpenseStage _fromStage = TravelExpenseStage.AssignedForPayment;

        public bool CanHandle(string key)
        {
            return key == Globals.AssignedForPaymentFinal;
        }

        public void Process(TravelExpenseEntity newte)
        {
            newte.Finalize();
        }
    }
}