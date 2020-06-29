using System;
using Polly;
using Polly.Retry;
using Serilog;

namespace SharedWouldBeNugets
{
    public class PolicyService : IPolicyService
    {
        private readonly int _retriesKatas = 15;

        public PolicyService(ILogger logger)
        {
            DatabaseMigrationAndSeedingPolicy = Policy
                .Handle<Exception>(e =>
                {
                    logger.Warning(e, "During DatabaseMigrationAndSeedingPolicy");
                    return true;
                })
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            KataApiRetryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(_retriesKatas, i => TimeSpan.FromSeconds(10), (exception, span, retryCount, context) =>
                {
                    logger.Warning($"Retry:{retryCount} of {_retriesKatas}");
                });
        }

        public AsyncRetryPolicy DatabaseMigrationAndSeedingPolicy { get; }
        public AsyncRetryPolicy KataApiRetryPolicy { get; }
    }
}