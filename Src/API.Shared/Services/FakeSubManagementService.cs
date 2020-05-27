using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace API.Shared.Services
{
    public class FakeSubManagementService : ISubManagementService
    {
        public FakeSubManagementService(string sub)
        {
            Sub = sub;
        }

        public string GetSub(ClaimsPrincipal claimsPrincipal, HttpContext httpContext)
        {
            return Sub;
        }

        public string Sub { get; set; }
    }
}