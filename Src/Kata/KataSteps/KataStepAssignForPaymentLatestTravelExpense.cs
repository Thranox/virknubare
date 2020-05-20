using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

namespace Kata.KataSteps
{
    public class KataStepAssignForPaymentLatestTravelExpense : KataStepBase, IKataStep
    {
        private readonly ILogger _logger;
        private readonly IRestClientProvider _restClientProvider;

        public KataStepAssignForPaymentLatestTravelExpense(ILogger logger, IRestClientProvider restClientProvider,
            IClientContext clientContext) : base(clientContext)
        {
            _logger = logger;
            _restClientProvider = restClientProvider;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "AssignForPaymentLatestTravelExpense";
        }

        protected override async Task Execute(string nameOfLoggedInUser)
        {
            var allTravelExpenseDtos = ClientContext.TravelExpenseGetResponse.Result;
            var idOfLatestCreation = ClientContext.TravelExpenseCreateResponse.Id;

            var latestCreated = allTravelExpenseDtos.Single(x => x.Id == idOfLatestCreation);

            // For now -- we only have one flowstep for each stage
            var allowedFlowDto = latestCreated.AllowedFlows.First();

            _logger.Debug("Applying flowstep to TravelExpense: " + allowedFlowDto.Description);
            var restClient = _restClientProvider.GetRestClient(nameOfLoggedInUser);
            var restRequest = new RestRequest(
                    new Uri($"/travelexpenses/{latestCreated.Id}/FlowStep/{allowedFlowDto.FlowStepId}",
                        UriKind.Relative)
                )
                ;
            var travelExpenseProcessStepResponse =
                await restClient.PostAsync<TravelExpenseProcessStepResponse>(restRequest);
            ClientContext.TravelExpenseProcessStepResponse = travelExpenseProcessStepResponse;
            _logger.Debug("Applied flowstep to TravelExpense: ",
                JsonConvert.SerializeObject(travelExpenseProcessStepResponse));
        }
    }
}