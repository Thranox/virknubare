using System.Security.Claims;

namespace API.Shared.Services
{
    public interface ISubManagementService
    {
        string GetSub(ClaimsPrincipal userIdentity);
    }
}