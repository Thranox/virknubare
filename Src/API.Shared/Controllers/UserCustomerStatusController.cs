using System;
using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Shared.Controllers
{
    [ApiController]
    [Route("usercustomerstatus")]
    public class UserCustomerStatusController : ControllerBase
    {
        private readonly ISubManagementService _subManagementService;
        private readonly IUserCustomerStatusService _userCustomerStatusService;

        public UserCustomerStatusController(ISubManagementService subManagementService,
            IUserCustomerStatusService userCustomerStatusService)
        {
            _subManagementService = subManagementService;
            _userCustomerStatusService = userCustomerStatusService;
        }

        [HttpPut]
        [Route("{userId}/{customerId}/{userStatus}")]
        public async Task<ActionResult<UserCustomerPutResponse>> Put(Guid userId, Guid customerId, int userStatus)
        {
            var sub = _subManagementService.GetSub(User, HttpContext);

            var flowStepGetResponse = await _userCustomerStatusService.PutAsync(sub, userId, customerId, userStatus);

            return Ok(flowStepGetResponse);
        }
    }
}