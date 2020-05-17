using System;
using System.Threading.Tasks;
using Application.Dtos;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

namespace Kata
{
    public class KataStepGetAllTravelExpenses : KataStepBase, IKataStep
    {
        private readonly IClientContext _clientContext;
        private readonly ILogger _logger;
        private readonly IRestClientProvider _restClientProvider;

        public KataStepGetAllTravelExpenses(ILogger logger, IRestClientProvider restClientProvider,
            IClientContext clientContext) : base(clientContext)
        {
            _logger = logger;
            _restClientProvider = restClientProvider;
            _clientContext = clientContext;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "GetAllTravelExpenses";
        }

        protected override async Task Execute(Properties properties, string nameOfLoggedInUser)
        {
            // As Alice (politician), get all Travel Expenses (that is, all she can see)
            _logger.Debug("Getting TravelExpenses...");
            var restClient = _restClientProvider.GetRestClient(nameOfLoggedInUser);
            var travelExpenseGetResponse =
                await restClient.GetAsync<TravelExpenseGetResponse>(
                    new RestRequest(new Uri("/travelexpenses", UriKind.Relative)));
            _logger.Debug("travelExpenseGetResponse - {travelExpenseGetResponse}",
                JsonConvert.SerializeObject(travelExpenseGetResponse));
            _clientContext.TravelExpenseGetResponse = travelExpenseGetResponse;
        }
    }
}