using System;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Shared.Controllers
{
    [ApiController]
    [Route("usercustomerstatus")]
    public class UserCustomerStatusController : ControllerBase
    {
        private readonly IUserCustomerStatusService _userCustomerStatusService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubManagementService _subManagementService;

        public UserCustomerStatusController(ISubManagementService subManagementService, IUserCustomerStatusService userCustomerStatusService, IUnitOfWork unitOfWork)
        {
            _subManagementService = subManagementService;
            _userCustomerStatusService = userCustomerStatusService;
            _unitOfWork = unitOfWork;
        }

        [HttpPut]
        [Route("Put/{userId}/{customerId}/{userStatus}")]
        public async Task<ActionResult<FlowStepGetResponse>> Put(Guid userId, Guid customerId, string userStatus)
        {
            var sub = _subManagementService.GetSub(User);
            var userEntity = _unitOfWork.Repository.List(new UserBySub(sub)).SingleOrDefault();

            var userIsAdmin = userEntity
                .CustomerUserPermissions
                .Any(x =>x.CustomerId == customerId && x.UserStatus == UserStatus.UserAdministrator);

            if(!userIsAdmin)
                throw new BusinessRuleViolationException(userId, "Can't change as calling user is not admin.");

            var flowStepGetResponse = await _userCustomerStatusService.PutAsync(sub, userId, customerId,userStatus);

            return Ok(flowStepGetResponse);
        }
    }
}