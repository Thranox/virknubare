using System;
using System.Threading.Tasks;
using Application.Dtos;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

namespace Kata.KataSteps
{
    public class KataStepGetSingleTravelExpense : KataStepBase, IKataStep
    {
        private readonly ILogger _logger;
        private readonly IRestClientProvider _restClientProvider;

        public KataStepGetSingleTravelExpense(ILogger logger, IRestClientProvider restClientProvider,
            IClientContext clientContext) : base(clientContext)
        {
            _logger = logger;
            _restClientProvider = restClientProvider;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "GetSingleTravelExpense";
        }

        protected override async Task Execute(string nameOfLoggedInUser)
        {
            // Still alice, create new Travel Expense
            _logger.Debug("Getting single TravelExpense...");
            var restClient = _restClientProvider.GetRestClient(nameOfLoggedInUser);
            var travelExpenseId = ClientContext.TravelExpenseCreateResponse.Id;
            var restRequest = new RestRequest(
                new Uri("/travelexpenses/"+travelExpenseId, UriKind.Relative)
            );
            var travelExpenseGetByIdResponse = await restClient.GetAsync<TravelExpenseGetByIdResponse>(restRequest);
            ClientContext.TravelExpenseGetByIdResponse = travelExpenseGetByIdResponse;
            _logger.Debug("Got TravelExpense {travelExpenseGetByIdResponse}",JsonConvert.SerializeObject(travelExpenseGetByIdResponse));
        }
    }
}