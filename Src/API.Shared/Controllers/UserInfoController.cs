using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Shared.Controllers
{
    [ApiController]
    [Route("userinfo")]
    public class UserInfoController : ControllerBase
    {
        private readonly IGetUserInfoService _getUserInfoService;
        private readonly ISubManagementService _subManagementService;
        private readonly IInvitationService _invitationService;

        public UserInfoController(ISubManagementService subManagementService,
            IGetUserInfoService getUserInfoService, IInvitationService invitationService)
        {
            _subManagementService = subManagementService;
            _getUserInfoService = getUserInfoService;
            _invitationService = invitationService;
        }

        [HttpGet]
        public async Task<ActionResult<UserInfoGetResponse>> Get()
        {
            var polApiContext = await _subManagementService.GetPolApiContext(HttpContext);

            var acceptWaitingInvitations = await _invitationService.AcceptWaitingInvitations(polApiContext);
            
            var userInfoGetResponse = await _getUserInfoService.GetAsync(polApiContext);

            return Ok(userInfoGetResponse);
        }
    }
}