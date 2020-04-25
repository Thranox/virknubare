using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace API.Shared.Services
{
    public class FakeSubManagementService : ISubManagementService
    {
        private readonly IConfiguration _configuration;

        public FakeSubManagementService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetSub(ClaimsPrincipal claimsPrincipal)
        {
            return _configuration.GetValue<string>("SubUsedWhenAuthenticationDisabled");
        }
    }
}