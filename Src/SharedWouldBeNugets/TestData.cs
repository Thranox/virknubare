using System;
using System.Collections.Generic;
using System.Security.Claims;
using Domain.ValueObjects;
using IdentityModel;
using IdentityServer4;

namespace SharedWouldBeNugets
{
    public static class TestData
    {
        public const string DummyCustomerName1 = "Dummy Customer";
        public const string DummyCustomerName2 = "Dummy2 Customer2";

        public const string DummyPolSubAlice = "6E9D6247-8436-420B-A542-55B97B8B05E0";
        public const string DummySekSubBob = "6E9D6247-8436-420B-A542-55B97B8B05E1";
        public const string DummyLedSubCharlie = "6E9D6247-8436-420B-A542-55B97B8B05E2";
        public const string DummyAdminSubDennis = "6E9D6247-8436-420B-A542-55B97B8B05E3";
        public const string DummyInitialSubEdward = "6E9D6247-8436-420B-A542-55B97B8B05E4";
        public const string DummyInitialSubFreddie = "6E9D6247-8436-420B-A542-55B97B8B05E5";

        private static readonly Dictionary<TravelExpenseStage, string> FlowStepDescriptionNameByStage =
            new Dictionary<TravelExpenseStage, string>
            {
                {TravelExpenseStage.Initial, "Færdigmeld"},
                {TravelExpenseStage.ReportedDone, "Attester"},
                {TravelExpenseStage.Certified, "Anvis afregning"},
                {TravelExpenseStage.AssignedForPayment, "Udbetal"}
            };

        public static IEnumerable<PolTestUser> GetTestUsers()
        {
            yield return new PolTestUser("alice", new PolUserCapabilities().AddCanCreateTravelExpense());
            yield return new PolTestUser("bob", new PolUserCapabilities().AddCanCreateTravelExpense());
            yield return new PolTestUser("charlie", new PolUserCapabilities().AddCanCreateTravelExpense());
            yield return new PolTestUser("dennis", new PolUserCapabilities().AddCanCreateTravelExpense());
            yield return new PolTestUser("edward", new PolUserCapabilities().AddCanCreateTravelExpense());
            yield return new PolTestUser("freddie", new PolUserCapabilities().AddCanCreateTravelExpense());
        }

        public static IEnumerable<Claim> GetClaimsByUserName(string userName)
        {
            if (userName == "alice")
                return new[]
                {
                    new Claim(ImproventoGlobals.ImproventoSubClaimName, DummyPolSubAlice),
                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                    new Claim(JwtClaimTypes.Address,
                        @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                        IdentityServerConstants.ClaimValueTypes.Json)
                };
            if (userName == "bob")
                return new[]
                {
                    new Claim(ImproventoGlobals.ImproventoSubClaimName, DummySekSubBob),
                    new Claim(JwtClaimTypes.Name, "Bob Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Bob"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                    new Claim(JwtClaimTypes.Address,
                        @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                        IdentityServerConstants.ClaimValueTypes.Json),
                    new Claim("location", "somewhere")
                };
            if (userName == "charlie")
                return new[]
                {
                    new Claim(ImproventoGlobals.ImproventoSubClaimName, DummyLedSubCharlie),
                    new Claim(JwtClaimTypes.Name, "Charlie Brown"),
                    new Claim(JwtClaimTypes.GivenName, "Charlie"),
                    new Claim(JwtClaimTypes.FamilyName, "Brown"),
                    new Claim(JwtClaimTypes.Email, "CharlieBrown@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://charlie.com"),
                    new Claim(JwtClaimTypes.Address,
                        @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                        IdentityServerConstants.ClaimValueTypes.Json),
                    new Claim("location", "somewhere")
                };
            if (userName == "dennis")
                return new[]
                {
                    new Claim(ImproventoGlobals.ImproventoSubClaimName, DummyAdminSubDennis),
                    new Claim(JwtClaimTypes.Name, "Dennis The Menace"),
                    new Claim(JwtClaimTypes.GivenName, "Dennis"),
                    new Claim(JwtClaimTypes.FamilyName, "Menace"),
                    new Claim(JwtClaimTypes.Email, "DennisTheMenace@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://dennis.com"),
                    new Claim(JwtClaimTypes.Address,
                        @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                        IdentityServerConstants.ClaimValueTypes.Json),
                    new Claim("location", "somewhere")
                };
            if (userName == "edward")
                return new[]
                {
                    new Claim(ImproventoGlobals.ImproventoSubClaimName, DummyInitialSubEdward),
                    new Claim(JwtClaimTypes.Name, "Edward Norton"),
                    new Claim(JwtClaimTypes.GivenName, "Edward"),
                    new Claim(JwtClaimTypes.FamilyName, "Norton"),
                    new Claim(JwtClaimTypes.Email, "EdwardNorton@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://edward.com"),
                    new Claim(JwtClaimTypes.Address,
                        @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                        IdentityServerConstants.ClaimValueTypes.Json),
                    new Claim("location", "somewhere")
                };
            if (userName == "freddie")
                return new[]
                {
                    new Claim(ImproventoGlobals.ImproventoSubClaimName, DummyInitialSubFreddie),
                    new Claim(JwtClaimTypes.Name, "Freddie Mercury"),
                    new Claim(JwtClaimTypes.GivenName, "Freddie"),
                    new Claim(JwtClaimTypes.FamilyName, "Mercury"),
                    new Claim(JwtClaimTypes.Email, "FreddieMercury@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://freddie.com"),
                    new Claim(JwtClaimTypes.Address,
                        @"{ 'street_address': 'One Great Singer', 'locality': 'Heaven', 'postal_code': 69118, 'country': 'Beyond' }",
                        IdentityServerConstants.ClaimValueTypes.Json),
                    new Claim("location", "somewhere")
                };
            throw new NotImplementedException();
        }

        public static IEnumerable<TestCustomer> GetTestCustomers()
        {
            yield return new TestCustomer {Name = DummyCustomerName1};
            yield return new TestCustomer {Name = DummyCustomerName2};
        }

        public static string GetFlowStepDescription(TravelExpenseStage travelExpenseStage)
        {
            return FlowStepDescriptionNameByStage[travelExpenseStage];
        }

        public static int GetNumberOfTestDataTravelExpenses()
        {
            return 9;
        }
    }
}