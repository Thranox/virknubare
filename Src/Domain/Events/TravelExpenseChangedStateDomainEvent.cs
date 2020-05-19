using Domain.Entities;
using Domain.SharedKernel;

namespace Domain.Events
{
    public class TravelExpenseChangedStateDomainEvent : BaseDomainEvent
    {
        public StageEntity StageBefore { get; }
        public TravelExpenseEntity TravelExpenseEntity { get; }
        public UserEntity UserEntityMakingChange { get; }

        public TravelExpenseChangedStateDomainEvent(StageEntity stageBefore,
            TravelExpenseEntity travelExpenseEntity, UserEntity userEntityMakingChange)
        {
            StageBefore = stageBefore;
            TravelExpenseEntity = travelExpenseEntity;
            UserEntityMakingChange = userEntityMakingChange;
        }
    }
}