using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using SharedWouldBeNugets;

namespace Kata
{
    public class KataStepVerifySwaggerUp : IKataStep
    {
        private readonly ILogger _logger;

        public KataStepVerifySwaggerUp(ILogger logger)
        {
            _logger = logger;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "VerifySwaggerUp";
        }
        public async Task ExecuteAsync(Properties properties, string nameOfLoggedInUser)
        {
            // Wait for api being up
            var kataApiRetryPolicy = new PolicyService(_logger).KataApiRetryPolicy;
            await kataApiRetryPolicy.Execute(async () =>
            {
                try
                {
                    _logger.Information("Trying to reach swagger page...");
                    var cancellationTokenSource = new CancellationTokenSource(100);
                    var httpClient = new HttpClient();
                    await httpClient.GetAsync(new Uri(properties.ApiEndpoint + "/swagger/index.html"), cancellationTokenSource.Token);
                    _logger.Information("Done trying to reach swagger page...");
                }
                catch (TaskCanceledException e)
                {
                    _logger.Debug("Timeout");
                }
            });
        }
    }
}