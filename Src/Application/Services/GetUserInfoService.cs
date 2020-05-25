using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using Domain.Entities;

namespace Application.Services
{
    public class GetUserInfoService : IGetUserInfoService
    {
        public async Task<UserInfoGetResponse> GetAsync(PolApiContext polApiContext)
        {
            return await Task.FromResult(new UserInfoGetResponse
            {
                UserCustomerInfo = GetUserCustomerInfo(polApiContext.CallingUser.CustomerUserPermissions)
                    .ToArray()
            });
        }

        private IEnumerable<UserCustomerInfo> GetUserCustomerInfo(
            ICollection<CustomerUserPermissionEntity> customerUserPermissions)
        {
            foreach (var customerUserPermissionEntity in customerUserPermissions)
                yield return new UserCustomerInfo
                {
                    CustomerId = customerUserPermissionEntity.CustomerId,
                    CustomerName = customerUserPermissionEntity.Customer.Name,
                    UserCustomerStatusText = Globals.UserStatusNamesDanish[customerUserPermissionEntity.UserStatus],
                    UserCustomerStatus = (int) customerUserPermissionEntity.UserStatus
                };
        }
    }
}