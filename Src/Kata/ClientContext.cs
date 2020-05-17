using Application.Dtos;

namespace Kata
{
    public class ClientContext:IClientContext
    {
        public UserInfoGetResponse UserInfoGetResponse { get; set; }
        public TravelExpenseGetResponse TravelExpenseGetResponse { get; set; }
        public TravelExpenseCreateResponse TravelExpenseCreateResponse { get; set; }
        public FlowStepGetResponse FlowStepGetResponse { get; set; }
    }
}