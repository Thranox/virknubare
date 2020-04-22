using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace IDP.Services
{
    public class CustomProfileService : IProfileService
    {
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = TestUsers.Users.SingleOrDefault(x => x.SubjectId == sub);
            var principal = new ClaimsPrincipal(
                new ClaimsIdentity[]
                {
                    new ClaimsIdentity(user.Claims.ToArray())
                });

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            //if (user.Email == "admin@globomantics.com")
            //{
            //    claims.Add(new Claim(JwtClaimTypes.Role, "Admin"));
            //}
            context.IssuedClaims = claims;

            await Task.CompletedTask;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = TestUsers.Users.SingleOrDefault(x=>x.SubjectId==sub);
            context.IsActive = user != null;
            await Task.CompletedTask;
        }
    }
}