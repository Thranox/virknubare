using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServerAspNetIdentit.Models;

namespace IDP
{
    public class EmailFactory : IEmailFactory
    {
        public EmailMessage CreateConfirmationEmail(ApplicationUser user, List<Claim> claims)
        {
            return new EmailMessage()
            {
                Subject = "Bekræft din oprettelse som bruger på Politikerafregning",
                Body = $"Hej {claims.Single(x=>x.Type== JwtClaimTypes.Email).Value},\n"+
                       $""
            };
        }
    }
}