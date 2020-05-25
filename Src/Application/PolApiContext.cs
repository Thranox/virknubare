using Domain.Entities;

namespace Application
{
    public class PolApiContext
    {
        public PolApiContext(UserEntity callingUser, string requestedUrl, PolSystem system)
        {
            CallingUser = callingUser;
            RequestedUrl = requestedUrl;
            System = system;
        }

        public UserEntity CallingUser { get; }
        public string RequestedUrl { get; }
        public PolSystem System { get; }
    }
}