using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Interfaces
{
    public interface IStageService
    {
        StageEntity GetStage(TravelExpenseStage travelExpenseStage);
    }
}