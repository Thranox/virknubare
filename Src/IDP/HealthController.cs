using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace IDP
{
    [AllowAnonymous]
    [Route("Health")]
    public class HealthController : Controller
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        [HttpGet("ShallowCheck")]
        public async Task<IActionResult> ShallowCheck()
        {
            _logger.LogInformation("Health ShallowCheck!");

            return await Task.FromResult(Ok("Health Shallow Check -- ok"));
        }

        [HttpGet("DeepCheck")]
        public async Task<IActionResult> DeepCheck()
        {
            _logger.LogInformation("Health DeepCheck!");

            //try
            //{
            //    Log.Logger.Information("Migrating UserIdentityDbContext");
            //    _userIdentityDbContext.Database.Migrate();
            //}
            //catch (Exception e)
            //{
            //    Log.Logger.Error(e, "During Migrating UserIdentityDbContext");
            //    throw;
            //}

            //try
            //{
            //    Log.Logger.Information("Migrating PersistedGrantDbContext");
            //    _persistedGrantDbContext.Database.Migrate();
            //}
            //catch (Exception e)
            //{
            //    Log.Logger.Error(e, "During Migrating PersistedGrantDbContext");
            //    throw;
            //}

            //try
            //{
            //    Log.Logger.Information("Migrating ConfigurationDbContext");
            //    _configurationDbContext.Database.Migrate();
            //}
            //catch (Exception e)
            //{
            //    Log.Logger.Error(e, "During Migrating ConfigurationDbContext");
            //    throw;
            //}

            return await Task.FromResult(Ok());
        }
    }
}