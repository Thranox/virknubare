using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Domain.ValueObjects;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

namespace Kata.KataSteps
{
    public class KataStepAddCustomersToUser : KataStepBase, IKataStep
    {
        private readonly ILogger _logger;
        private readonly IRestClientProvider _restClientProvider;

        public KataStepAddCustomersToUser(ILogger logger, IRestClientProvider restClientProvider,
            IClientContext clientContext) : base(clientContext)
        {
            _logger = logger;
            _restClientProvider = restClientProvider;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "AddCustomersToUser";
        }

        protected override async Task Execute(string nameOfLoggedInUser)
        {
            _logger.Debug("Creating CustomerUserPermission...");
            var restClient = _restClientProvider.GetRestClient(nameOfLoggedInUser);
            var restRequest = new RestRequest(
                    new Uri("/UserCustomerStatus", UriKind.Relative)
                )
                .AddJsonBody(new UserCustomerPostDto
                {
                    CustomerId = ClientContext.UserInfoGetResponse.UserCustomerInfo
                        .First(x => x.UserCustomerStatus != (int) UserStatus.Initial).CustomerId
                });
            var userCustomerPostResponse = await restClient.PostAsync<UserCustomerPostResponse>(restRequest);
            ClientContext.UserCustomerPostResponse = userCustomerPostResponse;
            _logger.Debug("Created CustomerUserPermission {userCustomerPostResponse}",
                JsonConvert.SerializeObject(userCustomerPostResponse));
        }
    }
}