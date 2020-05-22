using System;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using Serilog;

namespace SharedWouldBeNugets
{
    public class PolicyService : IPolicyService
    {
        public PolicyService(ILogger logger)
        {
            DatabaseMigrationAndSeedingPolicy = Policy
                .Handle<Exception>(e =>
                {
                    logger.Warning(e, "During DatabaseMigrationAndSeedingPolicy");
                    return true;
                })
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            KataApiRetryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(15, i => TimeSpan.FromSeconds(2), (exception, span,retryCount, context) =>
                {
                    logger.Warning($"Retry:{retryCount} of 5");
                });
        }

        public Policy DatabaseMigrationAndSeedingPolicy { get; }
        public AsyncRetryPolicy KataApiRetryPolicy { get; }
    }
}