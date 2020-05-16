using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Specifications;

namespace Application.Services
{
    public class GetUserInfoService:IGetUserInfoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserInfoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserInfoGetResponse> GetAsync(string sub)
        {
            var user = _unitOfWork
                .Repository
                .List(
                    new UserBySub(sub)
                    )
                .SingleOrDefault();
            if(user==null)
                throw new ItemNotFoundException(sub, "UserEntity");

            return await Task.FromResult( new UserInfoGetResponse
            {
                UserCustomerInfo = GetUserCustomerInfo(user.CustomerUserPermissions)
                    .ToArray()
            });
        }

        private IEnumerable<UserCustomerInfo> GetUserCustomerInfo(ICollection<CustomerUserPermissionEntity> customerUserPermissions)
        {
            foreach (var customerUserPermissionEntity in customerUserPermissions)
            {
                yield return new UserCustomerInfo()
                {
                    CustomerId = customerUserPermissionEntity.CustomerId,
                    CustomerName = customerUserPermissionEntity.Customer.Name,
                    UserCustomerStatusText =Globals.UserStatusNamesDanish[customerUserPermissionEntity.UserStatus],
                    UserCustomerStatus=(int) customerUserPermissionEntity.UserStatus
                };
            }
        }
    }
}