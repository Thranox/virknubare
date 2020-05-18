using System;
using System.Collections.Generic;
using System.Security.Claims;
using Domain.ValueObjects;
using IdentityModel;

namespace SharedWouldBeNugets
{
    public static class TestData
    {
        private static readonly Dictionary<TravelExpenseStage, string> FlowStepDescriptionNameByStage =
            new Dictionary<TravelExpenseStage, string>
            {
                {TravelExpenseStage.Initial, "Færdigmeld"},
                {TravelExpenseStage.ReportedDone, "Attester"},
                {TravelExpenseStage.Certified, "Anvis afregning"},
                {TravelExpenseStage.AssignedForPayment, "Udbetal"},
            };
        public const string DummyCustomerName = "Dummy Customer";
        public const string DummyPolSubAlice = "6E9D6247-8436-420B-A542-55B97B8B05E0";
        public const string DummySekSubBob = "6E9D6247-8436-420B-A542-55B97B8B05E1";
        public const string DummyLedSubCharlie = "6E9D6247-8436-420B-A542-55B97B8B05E2";

        public static IEnumerable< PolTestUser> GetTestUsers()
        {
            yield return new PolTestUser("alice", new PolUserCapabilities().AddCanCreateTravelExpense());
            yield return new PolTestUser("bob", new PolUserCapabilities().AddCanCreateTravelExpense());
        }

        public static IEnumerable<Claim> GetClaimsByUserName(string userName)
        {
            if (userName == "alice")
                return new[]
                {
                    new Claim(ImproventoGlobals.ImproventoSubClaimName, TestData.DummyPolSubAlice),
                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                    new Claim(JwtClaimTypes.Address,
                        @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                        IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                };
            if (userName == "bob")
                return new[]
                {
                    new Claim(ImproventoGlobals.ImproventoSubClaimName, TestData.DummySekSubBob),
                    new Claim(JwtClaimTypes.Name, "Bob Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Bob"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                    new Claim(JwtClaimTypes.Address,
                        @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                        IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                    new Claim("location", "somewhere")
                };
            if (userName == "charlie")
                return new[]
                {
                    new Claim(ImproventoGlobals.ImproventoSubClaimName, TestData.DummyLedSubCharlie),
                    new Claim(JwtClaimTypes.Name, "Charlie Brown"),
                    new Claim(JwtClaimTypes.GivenName, "Charlie"),
                    new Claim(JwtClaimTypes.FamilyName, "Brown"),
                    new Claim(JwtClaimTypes.Email, "CharlieBrown@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://charlie.com"),
                    new Claim(JwtClaimTypes.Address,
                        @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                        IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                    new Claim("location", "somewhere")
                };
            throw new NotImplementedException();
        }

        public static IEnumerable<TestCustomer> GetTestCustomers()
        {
            yield return new TestCustomer { Name = TestData.DummyCustomerName };
        }

        public static string GetFlowStepDescription(TravelExpenseStage travelExpenseStage)
        {
            return FlowStepDescriptionNameByStage[travelExpenseStage];
        }
    }
}