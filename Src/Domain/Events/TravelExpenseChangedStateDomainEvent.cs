using System;
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
            StageBefore = stageBefore??throw new ArgumentNullException(nameof(stageBefore));
            TravelExpenseEntity = travelExpenseEntity ?? throw new ArgumentNullException(nameof(travelExpenseEntity));
            if(travelExpenseEntity.OwnedByUser==null)
                throw new ArgumentNullException(nameof(travelExpenseEntity.OwnedByUser));
            UserEntityMakingChange = userEntityMakingChange ?? throw new ArgumentNullException(nameof(userEntityMakingChange));
        }
    }
}