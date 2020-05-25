using API.Shared.Controllers;
using Application.Dtos;

namespace Kata
{
    public class ClientContext:IClientContext
    {
        public UserInfoGetResponse UserInfoGetResponse { get; set; }
        public TravelExpenseGetResponse TravelExpenseGetResponse { get; set; }
        public TravelExpenseCreateResponse TravelExpenseCreateResponse { get; set; }
        public FlowStepGetResponse FlowStepGetResponse { get; set; }
        public TravelExpenseProcessStepResponse TravelExpenseProcessStepResponse { get; set; }
        public CustomerUserGetResponse CustomerUserGetResponse { get; set; }
        public UserCustomerPutResponse UserCustomerPutResponse { get; set; }
        public DatabaseResetResponse DatabaseResetResponse { get; set; }
        public CustomerInvitationsPostResponse CustomerInvitationsPostResponse { get; set; }
    }
}