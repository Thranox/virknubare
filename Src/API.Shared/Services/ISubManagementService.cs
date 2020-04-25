using System.Security.Claims;

namespace Web.Services
{
    public interface ISubManagementService
    {
        string GetSub(ClaimsPrincipal userIdentity);
    }
}