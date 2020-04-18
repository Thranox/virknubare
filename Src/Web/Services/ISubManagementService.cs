using System.Security.Claims;
using Domain.Entities;

namespace Web.Services
{
    public interface ISubManagementService
    {
        string GetSub(ClaimsPrincipal userIdentity);
    }
}