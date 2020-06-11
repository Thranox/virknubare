// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using SharedWouldBeNugets;

namespace IDP
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
                        "role",
                        ImproventoGlobals.ImproventoSubClaimName
                    }
                )
            };

        public static IEnumerable<ApiResource> Apis =>
            new[]
            {
                new ApiResource("teapi", "Travel Expense API", new List<string>
                {
                    JwtClaimTypes.Subject,
                    JwtClaimTypes.Name,
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified,
                    JwtClaimTypes.GivenName,
                    JwtClaimTypes.FamilyName,
                    JwtClaimTypes.Role,
                    ImproventoGlobals.ImproventoSubClaimName
                })
            };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                new Client
                {
                    AlwaysIncludeUserClaimsInIdToken = true,
                    ClientName = "Politikerafregning (Angular)",
                    ClientId = ImproventoGlobals.AngularClientId,
                    ClientSecrets = new List<Secret> {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RequireConsent = false,
                    RedirectUris = new List<string>
                    {
                        "https://localhost:44324/signin-redirect-callback",
                        "http://localhost:44324/signin-redirect-callback",
                        "http://localhost:4200/signin-redirect-callback",
                        "http://localhost:50627/signin-redirect-callback",
                        "https://localhost:44324/#/signin-redirect-callback",
                        "https://andersathome.dk/#/signin-redirect-callback",

                        "https://andersathome.dk/signin-redirect-callback",
                        "https://dev.politikerafregning.dk/signin-redirect-callback",
                        "https://politikerafregning.dk/signin-redirect-callback",

                        "https://andersathome.dk/#/signin-redirect-callback",
                        "https://dev.politikerafregning.dk/#/signin-redirect-callback",
                        "https://politikerafregning.dk/#/signin-redirect-callback",


                        ImproventoGlobals.LocalKataRedirect
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://localhost:44324/signout-redirect-callback",
                        "http://localhost:44324/signout-redirect-callback",
                        "http://localhost:4200/signout-redirect-callback",
                        "http://localhost:50627/signout-redirect-callback",
                        "https://localhost:44324/#/signout-redirect-callback",
                        "https://andersathome.dk/#/signout-redirect-callback",

                        "https://andersathome.dk/signout-redirect-callback",
                        "https://dev.politikerafregning.dk/signout-redirect-callback",
                        "https://politikerafregning.dk/signout-redirect-callback",

                        "https://andersathome.dk/#/signout-redirect-callback",
                        "https://dev.politikerafregning.dk/#/signout-redirect-callback",
                        "https://politikerafregning.dk/#/signout-redirect-callback",

                        ImproventoGlobals.LocalKataRedirect
                    },
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
                        "http://localhost:4200",
                        "http://localhost:50627",

                        "https://andersathome.dk",
                        "https://dev.politikerafregning.dk",
                        "https://politikerafregning.dk"
                    },
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 15 * 60 * 60 // 15 hrs
                    //AccessTokenLifetime = 730*24 * 60 * 60 // 2 yrs
                }
            };
    }
}