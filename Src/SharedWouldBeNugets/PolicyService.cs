using System;
using Polly;

namespace SharedWouldBeNugets
{
    public class PolicyService : IPolicyService
    {
        public PolicyService()
        {
            DatabaseMigrationAndSeedingPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public Policy DatabaseMigrationAndSeedingPolicy { get; }
    }
}