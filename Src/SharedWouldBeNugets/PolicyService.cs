using System;
using System.Threading.Tasks;
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

            KataApiRetryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(1),
                    delegate(Exception exception, TimeSpan span, int arg3, Context arg4)
                    {
                        logger.Warning("Retrying");
                    });
        }

        public Policy DatabaseMigrationAndSeedingPolicy { get; }
        public Policy KataApiRetryPolicy { get; }
    }
}