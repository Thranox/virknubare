using Polly;
using Polly.Retry;

namespace SharedWouldBeNugets
{
    public interface IPolicyService
    {
        Policy DatabaseMigrationAndSeedingPolicy { get; }
        AsyncRetryPolicy KataApiRetryPolicy { get; }
    }
}