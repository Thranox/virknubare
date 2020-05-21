using System.Threading.Tasks;
using API.Shared.Services;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Shared.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger _logger;
        private readonly ISubManagementService _subManagementService;

        public AdminController(ISubManagementService subManagementService, IAdminService adminService, ILogger logger)
        {
            _subManagementService = subManagementService;
            _adminService = adminService;
            _logger = logger;
        }

        [HttpPost("databasereset")]
        public async Task<ActionResult<DatabaseResetResponseDto>> DatabaseReset()
        {
            var sub = _subManagementService.GetSub(User);

            _logger.LogInformation("Reseeding database");
            await _adminService.ResetSeedningAsync();

            return Ok(new DatabaseResetResponseDto());
        }
    }

    public class DatabaseResetResponseDto
    {
    }
}