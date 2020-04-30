using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Domain.Services
{
    public class ProcessFlowStepAssignedForPaymentFinal : IProcessFlowStep
    {
        private readonly IStageService _stageService;

        public ProcessFlowStepAssignedForPaymentFinal(IStageService stageService)
        {
            _stageService = stageService;
        }

        public bool CanHandle(string key)
        {
            return key == Globals.AssignedForPaymentFinal;
        }

        public StageEntity GetResultingStage(TravelExpenseEntity travelExpenseEntity)
        {
            //BR: Can't be assigned payment if not certified:
            if (travelExpenseEntity.Stage.Value != TravelExpenseStage.AssignedForPayment)
                throw new BusinessRuleViolationException(travelExpenseEntity.Id,
                    "Rejseafregning kan ikke færdiggøres da den ikke er henvist til betaling.");

            travelExpenseEntity.Events.Add(new TravelExpenseUpdatedDomainEvent());

            return _stageService.GetStage(TravelExpenseStage.Final);
        }
    }
}