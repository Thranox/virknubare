using Polly.Retry;

namespace SharedWouldBeNugets
{
    public interface IPolicyService
    {
        AsyncRetryPolicy DatabaseMigrationAndSeedingPolicy { get; }
        AsyncRetryPolicy KataApiRetryPolicy { get; }
    }
}