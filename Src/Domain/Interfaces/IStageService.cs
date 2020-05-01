using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IStageService
    {
        StageEntity GetStage(TravelExpenseStage travelExpenseStage);
    }
}