using Domain.Entities;
using Domain.Interfaces;
using Domain.SharedKernel;

namespace Domain.Services
{
    public class ProcessFlowStepReportedDoneCertified : IProcessFlowStep
    {
        public bool CanHandle(string key)
        {
            return key == Globals.ReporteddoneCertified;
        }

        public TravelExpenseStage GetResultingStage(TravelExpenseEntity travelExpenseEntity)
        {
            //BR: Can't be certified if not reported done:
            if (travelExpenseEntity.Stage!=TravelExpenseStage.ReportedDone)
                throw new BusinessRuleViolationException(travelExpenseEntity.Id, "Rejseafregning kan ikke attesteres da den ikke er færdigmeldt.");

            travelExpenseEntity.Events.Add(new TravelExpenseUpdatedDomainEvent());

            return TravelExpenseStage.Certified;
        }
    }
}