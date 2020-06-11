using System;
using System.Linq;
using RestSharp;

namespace Kata
{
    public class RestClientProvider : IRestClientProvider
    {
        private readonly JwtUser[] _jwtUsers;
        private readonly Properties _properties;

        public RestClientProvider(Properties properties, JwtUser[] jwtUsers)
        {
            _properties = properties;
            _jwtUsers = jwtUsers;
        }

        public IRestClient GetRestClient(string jwtUserName)
        {
            IRestClient restClient = new RestClient(new Uri(_properties.ApiEndpoint))
            {
                Timeout = 200*1000,
                //TODO ThrowOnAnyError = true
            };
            var jwtUser = _jwtUsers.SingleOrDefault(x => x.Name == jwtUserName);
            if (jwtUser == null)
                throw new ArgumentException("Jwt not found in list", nameof(jwtUserName));
            var accessToken = jwtUser.AccessToken;
            restClient.AddDefaultHeader("Authorization",
                $"Bearer {accessToken}");
            return restClient;
        }
    }
}