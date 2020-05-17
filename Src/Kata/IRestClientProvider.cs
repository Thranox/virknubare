using RestSharp;

namespace Kata
{
    public interface IRestClientProvider
    {
        IRestClient GetRestClient(string jwtUserName);
    }
}