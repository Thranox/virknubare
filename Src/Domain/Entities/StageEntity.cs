using Domain.SharedKernel;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class StageEntity : BaseEntity
    {
        public StageEntity()
        {
            Value = TravelExpenseStage.Initial;
        }

        public StageEntity(TravelExpenseStage value) : this()
        {
            Value = value;
        }

        public TravelExpenseStage Value { get; set; }
    }
}