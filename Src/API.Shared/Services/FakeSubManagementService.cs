using System.Security.Claims;

namespace API.Shared.Services
{
    public class FakeSubManagementService : ISubManagementService
    {
        public FakeSubManagementService(string sub)
        {
            Sub = sub;
        }

        public string GetSub(ClaimsPrincipal claimsPrincipal)
        {
            return Sub;
        }

        public string Sub { get; set; }
    }
}