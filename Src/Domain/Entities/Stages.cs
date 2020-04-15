namespace Domain.Entities
{
    public static class Stages
    {
        public static StageEntity Initial { get; } = new StageEntity("Initial");

        public static StageEntity ReportedDone { get; } = new StageEntity("ReportedDone");

        public static StageEntity Certified { get; } = new StageEntity("Certified");

        public static StageEntity Final { get; } = new StageEntity("Final");
    }
}