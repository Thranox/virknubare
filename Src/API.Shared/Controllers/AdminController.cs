using System.Threading.Tasks;
using API.Shared.Services;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Shared.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly ISubManagementService _subManagementService;
        private readonly IAdminService _adminService;

        public AdminController(ISubManagementService subManagementService, IAdminService adminService)
        {
            _subManagementService = subManagementService;
            _adminService = adminService;
        }

        [HttpPost("databasereset")]
        public async Task<ActionResult> DatabaseReset()
        {
            var sub = _subManagementService.GetSub(User);

            await _adminService.ResetSeedningAsync();

            return Ok();
        }
    }

}