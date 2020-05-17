using System;
using System.Threading.Tasks;
using Application.Dtos;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

namespace Kata
{
    public class KataStepGetUserInfo : IKataStep
    {
        private readonly ILogger _logger;
        private readonly IRestClientProvider _restClientProvider;
        private readonly IClientContext _clientContext;

        public KataStepGetUserInfo(ILogger logger, IRestClientProvider restClientProvider, IClientContext clientContext)
        {
            _logger = logger;
            _restClientProvider = restClientProvider;
            _clientContext = clientContext;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "GetUserInfo";
        }
        public async Task ExecuteAsync(Properties properties)
        {
            // As Alice, get customers from the UserInfo endpoint
            _logger.Debug("Getting UserInfoGetResponse...");
            var restClient =_restClientProvider. GetRestClient("alice");
            var result =
                await restClient.ExecuteAsync<UserInfoGetResponse>(
                    new RestRequest(new Uri("/userinfo", UriKind.Relative), Method.GET));
            var userInfoGetResponse = JsonConvert.DeserializeObject<UserInfoGetResponse>(result.Content);
            _logger.Debug("userInfoGetResponse - {userInfoGetResponse}",
                JsonConvert.SerializeObject(userInfoGetResponse));
            _clientContext.UserInfoGetResponse= userInfoGetResponse;
        }
    }
}