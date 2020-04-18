using Domain.Entities;
using Domain.Exceptions;
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

            travelExpenseEntity.Events.Add(new TravelExpenseUpdatedDomainEvent());

            return TravelExpenseStage.AssignedForPayment;
        }
    }
}