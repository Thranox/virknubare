using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly.Retry;
using Serilog;
using SharedWouldBeNugets;

namespace Kata.KataSteps
{
    public class KataStepVerifySwaggerUp : KataStepBase, IKataStep
    {
        private readonly ILogger _logger;
        private readonly Properties _properties;
        private AsyncRetryPolicy _kataApiRetryPolicy;

        public KataStepVerifySwaggerUp(ILogger logger, IClientContext clientContext, Properties properties, IPolicyService policyService) : base(
            clientContext)
        {
            _logger = logger;
            _properties = properties;
            _kataApiRetryPolicy = policyService.KataApiRetryPolicy;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "VerifySwaggerUp";
        }

        protected override async Task Execute(string nameOfLoggedInUser)
        {
            // Wait for api being up
            await _kataApiRetryPolicy
                .ExecuteAsync(async () =>
                    {
                        _logger.Information("Trying to reach swagger page...");

                        // Wait for max half a second each time.
                        var cancellationTokenSource = new CancellationTokenSource(500);

                        var httpClient = new HttpClient();
                        await httpClient.GetAsync(new Uri(_properties.ApiEndpoint + "/swagger/index.html"),
                            cancellationTokenSource.Token);
                        _logger.Information("Done trying to reach swagger page...");
                    }
                );
        }
    }
}