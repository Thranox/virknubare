using System;
using System.Threading.Tasks;
using RestSharp;
using Serilog;

namespace Kata
{
    public class KataStepResetTestData : IKataStep
    {
        private readonly ILogger _logger;
        private readonly IRestClientProvider _restClientProvider;

        public KataStepResetTestData(ILogger logger, IRestClientProvider restClientProvider)
        {
            _logger = logger;
            _restClientProvider = restClientProvider;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "ResetTestData";
        }
        public async Task ExecuteAsync(Properties properties)
        {
            // Reset test data in database. This requires God access.
            _logger.Debug("Resetting Database...");
            var restClient =_restClientProvider.GetRestClient("alice");
            await restClient.PostAsync<object>(new RestRequest(new Uri("/Admin/DatabaseReset", UriKind.Relative)));
            _logger.Debug("Database reset.");
        }
    }
}