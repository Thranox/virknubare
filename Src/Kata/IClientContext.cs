using Application.Dtos;

namespace Kata
{
    public interface IClientContext
    {
        UserInfoGetResponse UserInfoGetResponse { get; set; }
        TravelExpenseGetResponse TravelExpenseGetResponse { get; set; }
        TravelExpenseCreateResponse TravelExpenseCreateResponse { get; set; }
        FlowStepGetResponse FlowStepGetResponse { get; set; }
        TravelExpenseProcessStepResponse TravelExpenseProcessStepResponse { get; set; }
        CustomerUserGetResponse CustomerUserGetResponse { get; set; }
        UserCustomerPutResponse UserCustomerPutResponse { get; set; }
        DatabaseResetResponse DatabaseResetResponse { get; set; }
        CustomerInvitationsPostResponse CustomerInvitationsPostResponse { get; set; }
        UserCustomerPostResponse UserCustomerPostResponse { get; set; }
        TravelExpenseUpdateResponse TravelExpenseUpdateResponse { get; set; }
        TravelExpenseGetByIdResponse TravelExpenseGetByIdResponse { get; set; }
    }
}