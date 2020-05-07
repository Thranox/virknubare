using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProcessFlowStep
    {
        bool CanHandle(string key);
        StageEntity GetResultingStage(TravelExpenseEntity travelExpenseEntity);
    }
}