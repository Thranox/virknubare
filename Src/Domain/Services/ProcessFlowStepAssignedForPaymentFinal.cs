using Domain.Entities;
using Domain.Interfaces;
using Domain.SharedKernel;

namespace Domain.Services
{
    public class ProcessFlowStepAssignedForPaymentFinal : IProcessFlowStep
    {
        public bool CanHandle(string key)
        {
            return key == Globals.AssignedForPaymentFinal;
        }

        public TravelExpenseStage GetResultingStage(TravelExpenseEntity travelExpenseEntity)
        {
            //BR: Can't be assigned payment if not certified:
            if (travelExpenseEntity.Stage!=TravelExpenseStage.AssignedForPayment)
                throw new BusinessRuleViolationException(travelExpenseEntity.Id, "Rejseafregning kan ikke færdiggøres da den ikke er henvist til betaling.");

            travelExpenseEntity.Events.Add(new TravelExpenseUpdatedDomainEvent());

            return TravelExpenseStage.Final;
        }
    }
}