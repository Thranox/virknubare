using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Specifications;
using Serilog;

namespace Application.Services
{
    public class UserCustomerStatusService : IUserCustomerStatusService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public UserCustomerStatusService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public Task<UserCustomerStatusPutResponse> PutAsync(string sub, Guid userId, Guid customerId, string userStatus)
        {
            var callingUserEntity = _unitOfWork.Repository.List(new UserBySub(sub)).SingleOrDefault();

            var callingUserIsAdmin = callingUserEntity
                .CustomerUserPermissions
                .Any(x => x.CustomerId == customerId && x.UserStatus == UserStatus.UserAdministrator);

            if (!callingUserIsAdmin)
                throw new BusinessRuleViolationException(callingUserEntity.Id, "Can't change as calling user is not admin (for customer).");

            var userToAdd = _unitOfWork.Repository.GetById<UserEntity>(userId);

            _logger.Information("Admin="+callingUserEntity.Name+ " UserId="+userId+ " customer="+customerId);

            var userStatuses = (UserStatus[]) Enum.GetValues(typeof(UserStatus));
            var status = userStatuses.SingleOrDefault(x => x.ToString() == userStatus);

            var customer = _unitOfWork.Repository.GetById<CustomerEntity>(customerId);

            customer.AddUser(userToAdd,status);

            throw new NotImplementedException();
        }
    }
}