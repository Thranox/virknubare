using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Specifications;

namespace Application.Services
{
    public class UserCustomerStatusService : IUserCustomerStatusService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserCustomerStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<UserCustomerStatusPutResponse> PutAsync(string sub, Guid userId, Guid customerId, string userStatus)
        {
            var callingUserEntity = _unitOfWork.Repository.List(new UserBySub(sub)).SingleOrDefault();

            var callingUserIsAdmin = callingUserEntity
                .CustomerUserPermissions
                .Any(x => x.CustomerId == customerId && x.UserStatus == UserStatus.UserAdministrator);

            if (!callingUserIsAdmin)
                throw new BusinessRuleViolationException(userId, "Can't change as calling user is not admin.");

            var userStatuses = (UserStatus[]) Enum.GetValues(typeof(UserStatus));

            throw new NotImplementedException();
        }
    }
}