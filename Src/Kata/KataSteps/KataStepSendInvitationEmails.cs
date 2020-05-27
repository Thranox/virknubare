using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Newtonsoft.Json;
using RestSharp;
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
            // Customer where this user is Administrator
            var customerId = ClientContext.UserInfoGetResponse.UserCustomerInfo.First(x => x.UserCustomerStatus == 2).CustomerId;
            _logger.Debug("Sending invitations for customer...");
            var restClient = _restClientProvider.GetRestClient(nameOfLoggedInUser);
            var restRequest = new RestRequest(new Uri($"customer/{customerId}/Invitations", UriKind.Relative), Method.POST );

            var customerInvitationsPostDto = new CustomerInvitationsPostDto {Emails = new[]{"user1@domain.com", "user2@domain.com"}};
            var jsonToSend = JsonConvert.SerializeObject(customerInvitationsPostDto);

            restRequest.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            restRequest.RequestFormat = DataFormat.Json;
            
            var result = await restClient.ExecuteAsync<CustomerInvitationsPostResponse>(restRequest);

            var customerInvitationsPostResponse = JsonConvert.DeserializeObject<CustomerInvitationsPostResponse>(result.Content);
            _logger.Debug("customerInvitationsPostResponse - {customerInvitationsPostResponse}",
                JsonConvert.SerializeObject(customerInvitationsPostResponse));
            ClientContext.CustomerInvitationsPostResponse = customerInvitationsPostResponse;
        }
    }
}