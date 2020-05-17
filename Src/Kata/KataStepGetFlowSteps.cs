using System;
using System.Threading.Tasks;
using Application.Dtos;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

namespace Kata
{
    public class KataStepGetFlowSteps : KataStepBase, IKataStep
    {
        private readonly ILogger _logger;
        private readonly IRestClientProvider _restClientProvider;

        public KataStepGetFlowSteps(ILogger logger, IRestClientProvider restClientProvider,
            IClientContext clientContext) : base(clientContext)
        {
            _logger = logger;
            _restClientProvider = restClientProvider;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "GetFlowSteps";
        }

        protected override async Task Execute(string nameOfLoggedInUser)
        {
            // As Alice (politician), get all Travel Expenses (that is, all she can see)
            _logger.Debug("Getting FlowSteps...");
            var restClient = _restClientProvider.GetRestClient(nameOfLoggedInUser);
            var flowStepGetResponse =
                await restClient.GetAsync<FlowStepGetResponse>(
                    new RestRequest(new Uri("/flowsteps", UriKind.Relative)));
            _logger.Debug("flowStepGetResponse - {flowStepGetResponse}",
                JsonConvert.SerializeObject(flowStepGetResponse));
            ClientContext.FlowStepGetResponse = flowStepGetResponse;
        }
    }
}