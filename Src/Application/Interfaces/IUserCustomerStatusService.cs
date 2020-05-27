using System;
using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Interfaces
{
    public interface IUserCustomerStatusService
    {
        Task<UserCustomerStatusPutResponse> PutAsync(PolApiContext polApiContext, Guid userId, Guid customerId, int userStatus);
    }
}