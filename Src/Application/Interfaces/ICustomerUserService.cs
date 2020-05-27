using System;
using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface ICustomerUserService
    {
        Task<CustomerUserGetResponse> GetAsync(PolApiContext sub, Guid customerId);
        Task<CustomerInvitationsPostResponse> CreateInvitationsAsync(PolApiContext polApiContext, Guid customerId,
            CustomerInvitationsPostDto customerInvitationsPostDto);
    }
}