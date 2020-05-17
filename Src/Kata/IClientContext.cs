using Application.Dtos;

namespace Kata
{
    public interface IClientContext
    {
        UserInfoGetResponse UserInfoGetResponse { get; set; }
        TravelExpenseGetResponse TravelExpenseGetResponse { get; set; }
        TravelExpenseCreateResponse TravelExpenseCreateResponse { get; set; }
        FlowStepGetResponse FlowStepGetResponse { get; set; }
    }
}