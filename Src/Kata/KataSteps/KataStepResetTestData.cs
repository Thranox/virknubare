using System;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Controllers;
using Application.Dtos;
using Domain.ValueObjects;
using RestSharp;
using Serilog;

namespace Kata.KataSteps
{
    public class KataStepResetTestData : KataStepBase, IKataStep
    {
        private readonly ILogger _logger;
        private readonly IRestClientProvider _restClientProvider;

        public KataStepResetTestData(ILogger logger, IRestClientProvider restClientProvider,
            IClientContext clientContext):base(clientContext)
        {
            _logger = logger;
            _restClientProvider = restClientProvider;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "ResetTestData";
        }

        protected override async Task Execute(string nameOfLoggedInUser)
        {
            // Reset test data in database. This requires God access.
            _logger.Debug("Resetting Database...");
            
            var restClient = _restClientProvider.GetRestClient(nameOfLoggedInUser);
            var restRequest = new RestRequest(
                    new Uri("admin/databasereset", UriKind.Relative)
                )
                ;
            var databaseResetResponseDto = await restClient.PostAsync<DatabaseResetResponse>(restRequest);
            ClientContext.DatabaseResetResponse = databaseResetResponseDto;
            _logger.Debug("Database reset.");
        }
    }
}