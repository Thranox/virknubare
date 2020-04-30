using Domain.Entities;

namespace Domain.Services
{
    public interface IStageService
    {
        StageEntity GetStage(TravelExpenseStage travelExpenseStage);
    }
}