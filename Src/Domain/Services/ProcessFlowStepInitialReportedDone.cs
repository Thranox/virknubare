using Domain.Entities;
using Domain.Interfaces;
using Domain.SharedKernel;

namespace Domain.Services
{
    public class ProcessFlowStepInitialReportedDone : IProcessFlowStep
    {
        public bool CanHandle(string key)
        {
            return key == Globals.InitialReporteddone;
        }

        public TravelExpenseStage GetResultingStage(TravelExpenseEntity travelExpenseEntity)
        {
            //BR: Can't be reported done unless...:
            //if (IsReportedDone) throw new BusinessRuleViolationException(Id, "Rejseafregning kan ikke �ndres n�r den er f�rdigmeldt.");
            //BR: Can't be certified if not reported done:
            if (travelExpenseEntity.IsReportedDone)
                throw new BusinessRuleViolationException(travelExpenseEntity.Id,
                    "Rejseafregning kan ikke f�rdigmeldes da den allerede er f�rdigmeldt.");

            travelExpenseEntity.Events.Add(new TravelExpenseUpdatedDomainEvent());

            return TravelExpenseStage.ReportedDone;
        }
    }
}