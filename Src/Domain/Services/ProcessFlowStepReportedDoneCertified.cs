using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Services
{
    public class ProcessFlowStepReportedDoneCertified : IProcessFlowStep
    {
        private readonly IStageService _stageService;

        public ProcessFlowStepReportedDoneCertified(IStageService stageService)
        {
            _stageService = stageService;
        }

        public bool CanHandle(string key)
        {
            return key == Globals.ReporteddoneCertified;
        }

        public StageEntity GetResultingStage(TravelExpenseEntity travelExpenseEntity)
        {
            //BR: Can't be certified if not reported done:
            if (travelExpenseEntity.Stage.Value != TravelExpenseStage.ReportedDone)
                throw new BusinessRuleViolationException(travelExpenseEntity.Id,
                    "Rejseafregning kan ikke attesteres da den ikke er færdigmeldt.");

            travelExpenseEntity.Events.Add(new TravelExpenseUpdatedDomainEvent());

            return _stageService.GetStage(TravelExpenseStage.Certified);
        }
    }
}