using Domain.Entities;
using Domain.Interfaces;
using Domain.SharedKernel;

namespace Domain.Services
{
    public class ProcessFlowStepCertifiedAssignedForPayment : IProcessFlowStep
    {
        public bool CanHandle(string key)
        {
            return key == Globals.CertifiedAssignedForPayment;
        }

        public TravelExpenseStage GetResultingStage(TravelExpenseEntity travelExpenseEntity)
        {
            //BR: Can't be assigned payment if not certified:
            if (travelExpenseEntity.Stage!=TravelExpenseStage.Certified)
                throw new BusinessRuleViolationException(travelExpenseEntity.Id, "Rejseafregning kan ikke anvises til betaling da den ikke er attesteret.");

            //BR: Can't be assigned payment if already assigned payment:
            if (travelExpenseEntity.Stage==TravelExpenseStage.AssignedForPayment)
                throw new BusinessRuleViolationException(travelExpenseEntity.Id, "Rejseafregning kan ikke anvises til betaling da den allerede er anvist til betaling.");

            travelExpenseEntity.Events.Add(new TravelExpenseUpdatedDomainEvent());

            return TravelExpenseStage.AssignedForPayment;
        }
    }
}