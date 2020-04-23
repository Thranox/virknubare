using System;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace IDP
{
    [AllowAnonymous]
    [SecurityHeaders]
    public class HealthController : Controller
    {
        private readonly ILogger<HealthController> _logger;
        private readonly UserIdentityDbContext _userIdentityDbContext;
        private readonly PersistedGrantDbContext _persistedGrantDbContext;
        private readonly ConfigurationDbContext _configurationDbContext;

        public HealthController(ILogger<HealthController> logger, UserIdentityDbContext userIdentityDbContext, PersistedGrantDbContext persistedGrantDbContext, ConfigurationDbContext configurationDbContext)
        {
            _logger = logger;
            _userIdentityDbContext = userIdentityDbContext;
            _persistedGrantDbContext = persistedGrantDbContext;
            _configurationDbContext = configurationDbContext;
        }

        [HttpGet("ShallowCheck")]
        public async Task<IActionResult> ShallowCheck()
        {
            _logger.LogInformation("Health Shallow Check!");

            return await Task.FromResult(Ok("Health Shallow Check -- ok"));
        }

        [HttpGet("DeepCheck")]
        public async Task<IActionResult> DeepCheck()
        {
            _logger.LogInformation("Health Shallow Check!");

            try
            {
                Log.Logger.Information("Migrating UserIdentityDbContext");
                _userIdentityDbContext.Database.Migrate();
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "During Migrating UserIdentityDbContext");
                throw;
            }

            try
            {
                Log.Logger.Information("Migrating PersistedGrantDbContext");
                _persistedGrantDbContext.Database.Migrate();
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "During Migrating PersistedGrantDbContext");
                throw;
            }

            try
            {
                Log.Logger.Information("Migrating ConfigurationDbContext");
                _configurationDbContext.Database.Migrate();
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "During Migrating ConfigurationDbContext");
                throw;
            }

            return await Task.FromResult(Ok());
        }
    }
}