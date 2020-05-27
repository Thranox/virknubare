using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SharedWouldBeNugets;

namespace API.Shared.Services
{
    public class SubManagementService : ISubManagementService
    {
        public string GetSub(ClaimsPrincipal claimsPrincipal, HttpContext httpContext)
        {
            var fullUrl = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(httpContext.Request);

            var userIdentity = claimsPrincipal.Identity;
            var claims = (userIdentity as ClaimsIdentity).Claims;
            var sub = claims.Single(x => x.Type == ImproventoGlobals.ImproventoSubClaimName).Value;
            return sub;
        }
    }
}