using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services
{
    public class ProcessFlowStepReportedDoneCertified : IProcessFlowStep
    {
        public bool CanHandle(string key)
        {
            return key == Globals.ReporteddoneCertified;
        }

        public void Process(TravelExpenseEntity newte)
        {
            newte.Certify();
        }
    }
}