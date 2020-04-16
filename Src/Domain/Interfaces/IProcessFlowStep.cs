using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProcessFlowStep
    {
        bool CanHandle(string key);
        TravelExpenseStage GetResultingStage(TravelExpenseEntity travelExpenseEntity);
    }
}