using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace API.Shared.Services
{
    public interface ISubManagementService
    {
        string GetSub(ClaimsPrincipal userIdentity, HttpContext httpContext);
    }
}