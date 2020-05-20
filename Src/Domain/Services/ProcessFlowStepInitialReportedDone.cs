using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.SharedKernel;
using Domain.ValueObjects;

namespace Domain.Services
{
    public class ProcessFlowStepInitialReportedDone : IProcessFlowStep
    {
        private readonly IStageService _stageService;

        public ProcessFlowStepInitialReportedDone(IStageService stageService)
        {
            _stageService = stageService;
        }

        public bool CanHandle(string key)
        {
            return key == Globals.InitialReporteddone;
        }

        public StageEntity GetResultingStage(TravelExpenseEntity travelExpenseEntity)
        {
            //BR: Can't be reported done unless...:
            //if (IsReportedDone) throw new BusinessRuleViolationException(Id, "Rejseafregning kan ikke ændres når den er færdigmeldt.");
            //BR: Can't be certified if not reported done:
            if (travelExpenseEntity.Stage.Value!=TravelExpenseStage.Initial)
                throw new BusinessRuleViolationException(travelExpenseEntity.Id,
                    "Rejseafregning kan ikke færdigmeldes da den allerede er færdigmeldt.");

            travelExpenseEntity.Events.Add(new TravelExpenseUpdatedDomainEvent());

            return _stageService.GetStage(TravelExpenseStage.ReportedDone);
        }
    }
}