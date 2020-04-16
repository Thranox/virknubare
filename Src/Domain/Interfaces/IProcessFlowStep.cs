using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProcessFlowStep
    {
        bool CanHandle(string key);
        void Process(TravelExpenseEntity newte);
    }
}