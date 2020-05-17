using Application.Dtos;

namespace Kata
{
    public class ClientContext:IClientContext
    {
        public UserInfoGetResponse UserInfoGetResponse { get; set; }
        public TravelExpenseGetResponse TravelExpenseGetResponse { get; set; }
    }
}