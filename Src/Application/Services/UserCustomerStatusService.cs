using System;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;

namespace Application.Services
{
    public class UserCustomerStatusService : IUserCustomerStatusService
    {
        public Task<UserCustomerStatusPutResponse> PutAsync(string sub, Guid userId, Guid customerId, string userStatus)
        {
            throw new NotImplementedException();
        }
    }
}