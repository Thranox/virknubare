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

        public UserInfoController(ISubManagementService subManagementService,
            IGetUserInfoService getUserInfoService)
        {
            _subManagementService = subManagementService;
            _getUserInfoService = getUserInfoService;
        }

        [HttpGet]
        public async Task<ActionResult<UserInfoGetResponse>> Get()
        {
            var sub = _subManagementService.GetSub(User);
            var userInfoGetResponse = await _getUserInfoService.GetAsync(sub);

            return Ok(userInfoGetResponse);
        }
    }
}