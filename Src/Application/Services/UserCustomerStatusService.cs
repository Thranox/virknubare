﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Services;
using Domain.Specifications;
using Serilog;

namespace Application.Services
{
    public class UserCustomerStatusService : IUserCustomerStatusService
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserStatusService _userStatusService;

        public UserCustomerStatusService(IUnitOfWork unitOfWork, ILogger logger, IUserStatusService userStatusService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userStatusService = userStatusService;
        }

        public async Task<UserCustomerStatusPutResponse> PutAsync(PolApiContext polApiContext, Guid userId, Guid customerId,
            int userStatus)
        {
            var customer = _unitOfWork
                               .Repository
                               .List(new CustomerById(customerId))
                               .SingleOrDefault() ??
                           throw new ArgumentException(nameof(customerId));

            var callingUserIsAdmin = polApiContext.CallingUser.IsUserAdminForCustomer(customerId);
            if (!callingUserIsAdmin)
                throw new BusinessRuleViolationException(polApiContext.CallingUser.Id,
                    "Can't change as calling user is not admin (for customer).");

            var userToAdd = _unitOfWork.Repository.GetById<UserEntity>(userId);

            _logger.Information("Admin=" + polApiContext.CallingUser.Name + " UserId=" + userId + " customer=" + customerId);

            var status = _userStatusService.GetUserStatusFromInt(userStatus);

            customer.SetUserStatus(userToAdd, status);

            await _unitOfWork.CommitAsync();

            return new UserCustomerStatusPutResponse();
        }
    }
}