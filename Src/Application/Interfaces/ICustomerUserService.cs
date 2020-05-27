using System;
using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface ICustomerUserService
    {
        Task<CustomerUserGetResponse> GetAsync(string sub, Guid customerId);
        Task<CustomerInvitationsPostResponse> CreateInvitationsAsync(string sub, Guid customerId,
            CustomerInvitationsPostDto customerInvitationsPostDto);
    }
}