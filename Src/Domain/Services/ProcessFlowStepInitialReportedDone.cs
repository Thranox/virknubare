using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services
{
    public class ProcessFlowStepInitialReportedDone : IProcessFlowStep
    {
        public bool CanHandle(string key)
        {
            return key ==Globals.InitialReporteddone;
        }

        public void Process(TravelExpenseEntity newte)
        {
            newte.ReportDone();
        }
    }
}