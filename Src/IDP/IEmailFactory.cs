using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServerAspNetIdentit.Models;

namespace IDP
{
    public interface IEmailFactory
    {
        EmailMessage CreateConfirmationEmail(ApplicationUser user, List<Claim> claims, Uri requestUri);
    }
}