using System;
using System.Threading.Tasks;
using Application.Dtos;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

namespace Kata.KataSteps
{
    public class KataStepGetUserInfo : KataStepBase, IKataStep
    {
        private readonly ILogger _logger;
        private readonly IRestClientProvider _restClientProvider;

        public KataStepGetUserInfo(ILogger logger, IRestClientProvider restClientProvider, IClientContext clientContext):base(clientContext)
        {
            _logger = logger;
            _restClientProvider = restClientProvider;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "GetUserInfo";
        }

        protected override async Task Execute(string nameOfLoggedInUser)
        {
            _logger.Debug("Getting UserInfoGetResponse...");
            var restClient = _restClientProvider.GetRestClient(nameOfLoggedInUser);
            var result =
                await restClient.ExecuteAsync<UserInfoGetResponse>(
                    new RestRequest(new Uri("/userinfo", UriKind.Relative), Method.GET));
            var userInfoGetResponse = JsonConvert.DeserializeObject<UserInfoGetResponse>(result.Content);
            _logger.Debug("userInfoGetResponse - {userInfoGetResponse}",
                JsonConvert.SerializeObject(userInfoGetResponse));
            ClientContext.UserInfoGetResponse = userInfoGetResponse;
        }
    }
}