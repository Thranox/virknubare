using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services
{
    public class ProcessFlowStepAssignedForPaymentFinal : IProcessFlowStep
    {
        public bool CanHandle(string key)
        {
            return key == Globals.AssignedForPaymentFinal;
        }

        public void Process(TravelExpenseEntity newte)
        {
            newte.Finalize();
        }
    }
}