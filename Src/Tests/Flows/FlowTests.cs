using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            var customer = new CustomerEntity();
            customer.Steps.Add(new FlowStepEntity {From = Stages.Initial, Key = "Initial -> ReportedDone"});
            customer.Steps.Add(new FlowStepEntity {From = Stages.ReportedDone, Key = "ReportedDone -> Certified"});
            customer.Steps.Add(new FlowStepEntity {From = Stages.Certified, Key = "Certified -> Final"});

            var newTe = new NewTe {Stage = Stages.Initial};

            do
            {
                Console.WriteLine("Stage: " + newTe.Stage.Description);
                var nextFlowSteps = customer.GetNextSteps(newTe.Stage).First();

                var processFlowStep = processFlowSteps
                    .SingleOrDefault(x => x.CanHandle(nextFlowSteps.Key));
                processFlowStep.Process(newTe);
            } while (newTe.Stage != Stages.Final);
        }
    }

    public interface IProcessFlowStep
    {
        bool CanHandle(string key);
        void Process(NewTe newte);
    }

    public class ProcessFlowStepInitialReportedDone : IProcessFlowStep
    {
        private readonly StageEntity _toStageEntity = Stages.ReportedDone;

        public bool CanHandle(string key)
        {
            return key == "Initial -> ReportedDone";
        }

        public void Process(NewTe newte)
        {
            newte.Stage = _toStageEntity;
        }
    }

    public class ProcessFlowStepReportedDoneCertified : IProcessFlowStep
    {
        private readonly StageEntity _toStageEntity = Stages.Certified;

        public bool CanHandle(string key)
        {
            return key == "ReportedDone -> Certified";
        }

        public void Process(NewTe newte)
        {
            newte.Stage = _toStageEntity;
        }
    }

    public class ProcessFlowStepCertifiedFinal : IProcessFlowStep
    {
        private readonly StageEntity _toStageEntity = Stages.Final;

        public bool CanHandle(string key)
        {
            return key == "Certified -> Final";
        }

        public void Process(NewTe newte)
        {
            newte.Stage = _toStageEntity;
        }
    }

    public class CustomerEntity
    {
        public CustomerEntity()
        {
            Steps = new List<FlowStepEntity>();
        }

        public ICollection<FlowStepEntity> Steps { get; }

        public IEnumerable<FlowStepEntity> GetNextSteps(StageEntity newTeStage)
        {
            return Steps
                .Where(x => x.From == newTeStage)
                .ToArray();
        }
    }

    public class FlowStepEntity
    {
        public string Key { get; set; }
        public StageEntity From { get; set; }
    }

    public class NewTe
    {
        public StageEntity Stage { get; set; }
    }

    public class StageEntity
    {
        public StageEntity(string description)
        {
            Description = description;
        }

        public string Description { get; }
    }

    public static class Stages
    {
        public static StageEntity Initial { get; } = new StageEntity("Initial");

        public static StageEntity ReportedDone { get; } = new StageEntity("ReportedDone");

        public static StageEntity Certified { get; } = new StageEntity("Certified");

        public static StageEntity Final { get; } = new StageEntity("Final");
    }
}