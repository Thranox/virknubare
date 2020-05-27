using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

namespace Kata.KataSteps
{
    public class KataStepChangeUserCustomerStatus : KataStepBase, IKataStep
    {
        private readonly ILogger _logger;
        private readonly IRestClientProvider _restClientProvider;

        public KataStepChangeUserCustomerStatus(ILogger logger, IRestClientProvider restClientProvider,
            IClientContext clientContext) : base(clientContext)
        {
            _logger = logger;
            _restClientProvider = restClientProvider;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "ChangeUserCustomerStatus";
        }

        protected override async Task Execute(string nameOfLoggedInUser)
        {
            var userPermissionDto =
                ClientContext.CustomerUserGetResponse.Users.Single(x => x.UserStatus == 0); // That would be Edward.
            var customerId = ClientContext.UserInfoGetResponse.UserCustomerInfo.First(x => x.UserCustomerStatus == 2)
                .CustomerId;
            var userStatus = 1; //Registered

            _logger.Debug("Changing UserCustomerStatus...");
            var restClient = _restClientProvider.GetRestClient(nameOfLoggedInUser);
            var result =
                await restClient.ExecuteAsync<UserCustomerPutResponse>(
                    new RestRequest(
                        new Uri($"usercustomerstatus/{userPermissionDto.UserId}/{customerId}/{userStatus}",
                            UriKind.Relative), Method.PUT));
            var userCustomerPutResponse = JsonConvert.DeserializeObject<UserCustomerPutResponse>(result.Content);
            _logger.Debug("userInfoGetResponse - {userInfoGetResponse}",
                JsonConvert.SerializeObject(userCustomerPutResponse));
            ClientContext.UserCustomerPutResponse = userCustomerPutResponse;
        }
    }
}

