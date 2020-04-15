using Domain.SharedKernel;

namespace Domain.Entities
{
    public class FlowStepEntity:BaseEntity
    {
        private FlowStepEntity()
        {

        }

        public FlowStepEntity(string key, TravelExpenseStage @from):this()
        {
            Key = key;
            From = @from;
        }
        public string Key { get; set; }
        public TravelExpenseStage From { get; set; }
    }
}