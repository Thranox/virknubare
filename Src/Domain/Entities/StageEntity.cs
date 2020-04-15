namespace Domain.Entities
{
    public class StageEntity
    {
        public StageEntity()
        {
            
        }
        public StageEntity(string description)
        {
            Description = description;
        }

        public string Description { get; }
    }
}