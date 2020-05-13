using System;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using Domain.Exceptions;
using Domain.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Shared.Controllers
{
    [ApiController]
    [Route("usercustomerstatus")]
    public class UserCustomerStatusController : ControllerBase
    {
        private readonly IUserCustomerStatusService _userCustomerStatusService;
        private readonly ISubManagementService _subManagementService;

        public UserCustomerStatusController(ISubManagementService subManagementService, IUserCustomerStatusService userCustomerStatusService)
        {
            _subManagementService = subManagementService;
            _userCustomerStatusService = userCustomerStatusService;
        }

        [HttpPut]
        [Route("Put/{userId}/{customerId}/{userStatus}")]
        public async Task<ActionResult<FlowStepGetResponse>> Put(Guid userId, Guid customerId, string userStatus)
        {
            var sub = _subManagementService.GetSub(User);

            var flowStepGetResponse = await _userCustomerStatusService.PutAsync(sub, userId, customerId,userStatus);

            return Ok(flowStepGetResponse);
        }
    }
}