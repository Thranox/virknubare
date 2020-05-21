using System;
using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface ICustomerUserService
    {
        Task<CustomerUserGetResponse> GetAsync(string sub, Guid customerId);
        Task<object> CreateInvitationsAsync(string sub, Guid customerId);
    }
}