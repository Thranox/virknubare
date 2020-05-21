using System.Threading.Tasks;
using Serilog;

namespace Kata.KataSteps
{
    public class KataStepSendInvitationEmails : KataStepBase, IKataStep
    {
        private readonly ILogger _logger;
        private readonly IRestClientProvider _restClientProvider;

        public KataStepSendInvitationEmails(ILogger logger, IRestClientProvider restClientProvider,
            IClientContext clientContext) : base(clientContext)
        {
            _logger = logger;
            _restClientProvider = restClientProvider;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "SendInvitationEmails";
        }

        protected override async Task Execute(string nameOfLoggedInUser)
        {
            //var allTravelExpenseDtos = ClientContext.TravelExpenseGetResponse.Result;
            //var idOfLatestCreation = ClientContext.TravelExpenseCreateResponse.Id;

            //var latestCreated = allTravelExpenseDtos.Single(x => x.Id == idOfLatestCreation);

            //// For now -- we only have one flowstep for each stage
            //var allowedFlowDto = latestCreated.AllowedFlows.First();

            //_logger.Debug("Applying flowstep to TravelExpense: " + allowedFlowDto.Description);
            //var restClient = _restClientProvider.GetRestClient(nameOfLoggedInUser);
            //var restRequest = new RestRequest(
            //        new Uri($"/travelexpenses/{latestCreated.Id}/FlowStep/{allowedFlowDto.FlowStepId}", UriKind.Relative)
            //    )
            //    ;
            //var travelExpenseProcessStepResponse = await restClient.PostAsync<TravelExpenseProcessStepResponse>(restRequest);
            //ClientContext.TravelExpenseProcessStepResponse = travelExpenseProcessStepResponse;
            //_logger.Debug("Applied flowstep to TravelExpense: ", JsonConvert.SerializeObject(travelExpenseProcessStepResponse));
        }
    }
}