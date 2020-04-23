using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IDP
{
    [AllowAnonymous]
    [SecurityHeaders]
    public class HealthController : Controller
    {
        private readonly ILogger<HealthController> _logger;
        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ShallowCheck()
        {
            _logger.LogInformation("Health Shallow Check!");

            return await Task.FromResult( Ok());
        }
    }
}