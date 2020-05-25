using Domain.Entities;

namespace API.Shared.Services
{
    public class PolApiContext
    {
        public PolApiContext(UserEntity callingUser, string requestedUrl)
        {
            CallingUser = callingUser;
            RequestedUrl = requestedUrl;
        }

        public UserEntity CallingUser { get; }
        public string RequestedUrl { get; }
    }
}