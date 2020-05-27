using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

namespace Kata.KataSteps
{
    public class KataStepGetCustomerUsers : KataStepBase, IKataStep
    {
        private readonly ILogger _logger;
        private readonly IRestClientProvider _restClientProvider;

        public KataStepGetCustomerUsers(ILogger logger, IRestClientProvider restClientProvider, IClientContext clientContext) : base(clientContext)
        {
            _logger = logger;
            _restClientProvider = restClientProvider;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "GetCustomerUsers";
        }

        protected override async Task Execute(string nameOfLoggedInUser)
        {
            // Customer where this user is Administrator
            var customerId = ClientContext.UserInfoGetResponse.UserCustomerInfo.First(x=>x.UserCustomerStatus==2).CustomerId;
            _logger.Debug("Getting Users for customer...");
            var restClient = _restClientProvider.GetRestClient(nameOfLoggedInUser);
            var result =
                await restClient.ExecuteAsync<CustomerUserGetResponse>(
                    new RestRequest(new Uri($"customer/{customerId}/Users", UriKind.Relative), Method.GET));
            var customerUserGetResponse = JsonConvert.DeserializeObject<CustomerUserGetResponse>(result.Content);
            _logger.Debug("customerUserGetResponse - {customerUserGetResponse}",
                JsonConvert.SerializeObject(customerUserGetResponse));
            ClientContext.CustomerUserGetResponse = customerUserGetResponse;
        }
    }

}