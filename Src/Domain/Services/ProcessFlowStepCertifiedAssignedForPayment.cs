using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Domain.Services
{
    public class ProcessFlowStepCertifiedAssignedForPayment : IProcessFlowStep
    {
        private readonly IStageService _stageService;

        public ProcessFlowStepCertifiedAssignedForPayment(IStageService stageService)
        {
            _stageService = stageService;
        }

        public bool CanHandle(string key)
        {
            return key == Globals.CertifiedAssignedForPayment;
        }

        public StageEntity GetResultingStage(TravelExpenseEntity travelExpenseEntity)
        {
            //BR: Can't be assigned payment if not certified:
            if (travelExpenseEntity.Stage.Value != TravelExpenseStage.Certified)
                throw new BusinessRuleViolationException(travelExpenseEntity.Id,
                    "Rejseafregning kan ikke anvises til betaling da den ikke er attesteret.");

            travelExpenseEntity.Events.Add(new TravelExpenseUpdatedDomainEvent());

            return _stageService.GetStage(TravelExpenseStage.AssignedForPayment);
        }
    }
}