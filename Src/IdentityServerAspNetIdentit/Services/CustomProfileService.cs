using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Serilog;

namespace IDP.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly UserIdentityDbContext _userIdentityDbContext;

        public CustomProfileService(UserIdentityDbContext userIdentityDbContext)
        {
            _userIdentityDbContext = userIdentityDbContext;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                var sub = context.Subject.GetSubjectId();
                Log.Logger.Debug("Getting profile claims for {sub}" + sub);
                var user = _userIdentityDbContext.Users.SingleOrDefault(x => x.SubjectId == sub);
                if (user == null)
                    throw new InvalidOperationException("User not found by sub: "+ sub);

                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.UserName)); // TODO: Register first and last name
                claims.Add(new Claim(ClaimTypes.Email, user.Email.ToLower()));

                claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

                context.IssuedClaims = claims;

            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "During GetProfileDataAsync()");
                throw;
            }
            await Task.CompletedTask;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = _userIdentityDbContext.Users.SingleOrDefault(x => x.SubjectId == sub);
            
            if (user == null)
                throw new InvalidOperationException("User not found by sub: " + sub);

            context.IsActive = user.EmailConfirmed && (user.LockoutEnd == null || user.LockoutEnd < DateTime.UtcNow);
            await Task.CompletedTask;
        }
    }
}