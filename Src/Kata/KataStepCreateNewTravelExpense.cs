using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Domain;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

namespace Kata
{
    public class KataStepCreateNewTravelExpense :KataStepBase, IKataStep
    {
        private readonly IClientContext _clientContext;
        private readonly ILogger _logger;
        private readonly IRestClientProvider _restClientProvider;

        public KataStepCreateNewTravelExpense(ILogger logger, IRestClientProvider restClientProvider,
            IClientContext clientContext) :base(clientContext)
        {
            _logger = logger;
            _restClientProvider = restClientProvider;
            _clientContext = clientContext;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "CreateNewTravelExpense";
        }

        protected override async Task Execute(string nameOfLoggedInUser)
        {
            // Still alice, create new Travel Expense
            _logger.Debug("Creating TravelExpense...");
            var restClient = _restClientProvider.GetRestClient(nameOfLoggedInUser);
            var restRequest = new RestRequest(
                    new Uri("/travelexpenses", UriKind.Relative)
                )
                .AddJsonBody(new TravelExpenseCreateDto
                {
                    Description = "From kata",
                    CustomerId = _clientContext.UserInfoGetResponse.UserCustomerInfo
                        .First(x => x.UserCustomerStatus != (int)UserStatus.Initial).CustomerId
                });
            var o = await restClient.PostAsync<TravelExpenseGetResponse>(restRequest);
            _logger.Debug("Created TravelExpense {object}", JsonConvert.SerializeObject(o));
        }
    }
}