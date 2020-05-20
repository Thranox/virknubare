using System;
using System.Threading.Tasks;
using Application.Dtos;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

namespace Kata.KataSteps
{
    public class KataStepGetAllTravelExpenses : KataStepBase, IKataStep
    {
        private readonly ILogger _logger;
        private readonly IRestClientProvider _restClientProvider;

        public KataStepGetAllTravelExpenses(ILogger logger, IRestClientProvider restClientProvider,
            IClientContext clientContext) : base(clientContext)
        {
            _logger = logger;
            _restClientProvider = restClientProvider;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "GetAllTravelExpenses";
        }

        protected override async Task Execute(string nameOfLoggedInUser)
        {
            _logger.Debug("Getting TravelExpenses...");
            var restClient = _restClientProvider.GetRestClient(nameOfLoggedInUser);
            var travelExpenseGetResponse =
                await restClient.GetAsync<TravelExpenseGetResponse>(
                    new RestRequest(new Uri("/travelexpenses", UriKind.Relative)));
            _logger.Debug("travelExpenseGetResponse - {travelExpenseGetResponse}",
                JsonConvert.SerializeObject(travelExpenseGetResponse));
            ClientContext.TravelExpenseGetResponse = travelExpenseGetResponse;
        }
    }
}