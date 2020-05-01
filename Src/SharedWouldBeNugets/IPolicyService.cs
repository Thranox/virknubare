using Polly;

namespace SharedWouldBeNugets
{
    public interface IPolicyService
    {
        Policy DatabaseMigrationAndSeedingPolicy { get; }
    }
}