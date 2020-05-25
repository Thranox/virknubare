using System;
using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface ICustomerUserService
    {
        Task<CustomerUserGetResponse> GetAsync(PolApiContext sub, Guid customerId);
        Task<CustomerInvitationsPostResponse> CreateInvitationsAsync(PolApiContext sub, Guid customerId,
            CustomerInvitationsPostDto customerInvitationsPostDto);
    }
}