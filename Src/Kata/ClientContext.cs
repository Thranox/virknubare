using Application.Dtos;

namespace Kata
{
    public class ClientContext:IClientContext
    {
        public UserInfoGetResponse UserInfoGetResponse { get; set; }
    }
}