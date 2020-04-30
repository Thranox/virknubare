﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServerAspNetIdentit
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles",
                    "Your roles(s)",
                    new List<string>
                    {
                        "role"
                    }
                )
            };

        public static IEnumerable<ApiResource> Apis =>
        new ApiResource[]
        {
            new ApiResource("teapi","Travel Expense API", new List<string>()
            {
                JwtClaimTypes.Subject,
                JwtClaimTypes.Name,
                JwtClaimTypes.Email,
                JwtClaimTypes.EmailVerified,
                JwtClaimTypes.GivenName,
                JwtClaimTypes.FamilyName,
                JwtClaimTypes.Role
            }){}
        };

        public static IEnumerable<Client> Clients =>
                new[]
                {
                new Client
                {
                    AlwaysIncludeUserClaimsInIdToken = true,
                    ClientName = "Politikerafregning (Angular)",
                    ClientId = "polangularclient",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RequireConsent = false,
                    RedirectUris = new List<string>{"https://localhost:44324/signin-redirect-callback"},
                    PostLogoutRedirectUris = new List<string>(){"https://localhost:44324/signout-redirect-callback"},
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "teapi"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        "https://localhost:44324",
                        "http://localhost:44324",
                        "http://localhost:4200"
                    },
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 15*60*60 // 15 hrs
                }
                };
    }
}