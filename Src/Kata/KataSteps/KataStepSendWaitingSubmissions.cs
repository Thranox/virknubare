using System;
using System.Threading.Tasks;
using RestSharp;
using Serilog;

namespace Kata.KataSteps
{
    public class KataStepSendWaitingSubmissions : KataStepBase, IKataStep
    {
        private readonly ILogger _logger;
        private readonly IRestClientProvider _restClientProvider;

        public KataStepSendWaitingSubmissions(ILogger logger, IRestClientProvider restClientProvider,
            IClientContext clientContext) : base(clientContext)
        {
            _logger = logger;
            _restClientProvider = restClientProvider;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "SendWaitingSubmissions";
        }

        protected override async Task Execute(string nameOfLoggedInUser)
        {
            _logger.Debug("Sending waiting submissions.");

            var restClient = _restClientProvider.GetRestClient(nameOfLoggedInUser);
            var restRequest = new RestRequest(
                    new Uri("/submissions/Tbd", UriKind.Relative)
                )
                ;
            var travelExpenseProcessStepResponse = await restClient.PostAsync<object>(restRequest);
            //ClientContext.TravelExpenseProcessStepResponse = travelExpenseProcessStepResponse;
            _logger.Debug("Done sending waiting submissions.");
        }
    }
}