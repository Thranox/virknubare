using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SharedWouldBeNugets
{
    public class PolTestUser
    {
        public Guid ImproventoSub
        {
            get
            {
                var claim = Claims.Single(x => x.Type == ImproventoGlobals.ImproventoSubClaimName);
                return new Guid(claim.Value);
            }
        }

        public PolUserCapabilities PolUserCapabilities { get; }

        public PolTestUser(string userName, PolUserCapabilities polUserCapabilities)
        {
            PolUserCapabilities = polUserCapabilities;
            Claims = TestData.GetClaimsByUserName(userName);
        }

        public IEnumerable<Claim> Claims { get;  }
    }
}