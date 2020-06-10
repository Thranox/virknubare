using System;
using System.Linq;
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
            var polApiContext = await _subManagementService.GetPolApiContext(HttpContext);

            var flowStepGetResponse = await _userCustomerStatusService.PutAsync(polApiContext, userId, customerId, userStatus);

            return Ok(flowStepGetResponse);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<UserCustomerPostResponse>> Post(UserCustomerPostDto userCustomerPostDto)
        {
            var polApiContext = await _subManagementService.GetPolApiContext(HttpContext);

            var ids = await _userCustomerStatusService
                .CreateCustomerStatusAsync(polApiContext, userCustomerPostDto.CustomerIds.ToArray());

            return Ok(ids);
        }
    }
}