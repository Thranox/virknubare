using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services
{
    public class ProcessFlowStepCertifiedAssignedForPayment : IProcessFlowStep
    {
        public bool CanHandle(string key)
        {
            return key == Globals.CertifiedAssignedForPayment;
        }

        public void Process(TravelExpenseEntity newte)
        {
            newte.AssignPayment();
        }
    }
}