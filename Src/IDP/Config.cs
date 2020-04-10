// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

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
                        "role"
                    }
                )
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                //new ApiResource
                //{
                //    Name = "projects-api",
                //    Enabled = true,
                //    Scopes = new List<Scope>
                //    {
                //        new Scope("projects-api")
                //    }
                //}
            };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                new Client
                {
                    ClientName = "Politikerafregning (Angular)",
                    ClientId = "polangularclient",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RedirectUris = new List<string>
                    {
                        "https://localhost:44324/signin-redirect-callback"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedCorsOrigins = new List<string> {"https://localhost:44324"}
                }
            };
    }
}