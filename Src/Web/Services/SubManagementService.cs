﻿using System.Linq;
using System.Security.Claims;
using Domain.Entities;

namespace Web.Services
{
    public class SubManagementService : ISubManagementService
    {
        public string GetSub(ClaimsPrincipal claimsPrincipal)
        {
            var userIdentity = claimsPrincipal.Identity;
            var claims = (userIdentity as ClaimsIdentity).Claims;
            var sub = claims.Single(x => x.Type == "sub").Value;
            return sub;
        }
    }
}