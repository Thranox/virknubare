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
            IRestClient restClient = new RestClient(new Uri(_properties.ApiEndpoint));
            restClient.AddDefaultHeader("Authorization",
                $"Bearer {_jwtUsers.Single(x => x.Name == jwtUserName).AccessToken}");
            return restClient;
        }
    }
}