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
    public class KataStepApproveLatestTravelExpense : KataStepBase, IKataStep
    {
        private readonly ILogger _logger;
        private readonly IRestClientProvider _restClientProvider;

        public KataStepApproveLatestTravelExpense(ILogger logger, IRestClientProvider restClientProvider,
            IClientContext clientContext) :base(clientContext)
        {
            _logger = logger;
            _restClientProvider = restClientProvider;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "ApproveLatestTravelExpense";
        }

        protected override async Task Execute(string nameOfLoggedInUser)
        {
            var allTravelExpenseDtos = ClientContext.TravelExpenseGetResponse.Result;
            var idOfLatestCreation = ClientContext.TravelExpenseCreateResponse.Id;

            var latestCreated = allTravelExpenseDtos.Single(x => x.Id == idOfLatestCreation);

            // For now -- we only have one flowstep for each stage
            var allowedFlowDto = latestCreated.AllowedFlows.First();

            _logger.Debug("Applying flowstep to TravelExpense: " +allowedFlowDto.Description);
            var restClient = _restClientProvider.GetRestClient(nameOfLoggedInUser);
            var restRequest = new RestRequest(
                    new Uri($"/travelexpenses/{latestCreated.Id}/FlowStep/{allowedFlowDto.FlowStepId}", UriKind.Relative)
                )
                ;
            var travelExpenseCreateResponse = await restClient.PostAsync<TravelExpenseCreateResponse>(restRequest);
            ClientContext.TravelExpenseCreateResponse = travelExpenseCreateResponse;
            _logger.Debug("Created TravelExpense {travelExpenseCreateResponse}", JsonConvert.SerializeObject(travelExpenseCreateResponse));
        }
    }
}