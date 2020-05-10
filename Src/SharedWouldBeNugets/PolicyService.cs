using System;
using Polly;
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
        }

        public Policy DatabaseMigrationAndSeedingPolicy { get; }
    }
}