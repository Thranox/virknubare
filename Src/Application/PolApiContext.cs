using System;
using Domain.Entities;

namespace Application
{
    public class PolApiContext
    {
        public PolApiContext(UserEntity callingUser, string requestedUrl, PolSystem system)
        {
            CallingUser = callingUser??throw new ArgumentNullException(nameof(callingUser));
            RequestedUrl = requestedUrl ?? throw new ArgumentNullException(nameof(requestedUrl));
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public UserEntity CallingUser { get; }
        public string RequestedUrl { get; }
        public PolSystem System { get; }
    }
}