using Domain.SharedKernel;

namespace Domain.Entities
{
    public class StageEntity : BaseEntity
    {
        public StageEntity()
        {
            Value = (int)TravelExpenseStage.Initial;
        }

        public StageEntity(in int value) : this()
        {
            Value = value;
        }

        public int Value { get; set; }
    }
}