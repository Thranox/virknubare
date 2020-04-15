namespace Domain.Entities
{
    public class FlowStepEntity
    {
        public string Key { get; set; }
        public StageEntity From { get; set; }
    }
}