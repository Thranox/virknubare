using Domain.ValueObjects;

namespace Domain.Services
{
    public interface IUserStatusService
    {
        UserStatus GetUserStatusFromString(string userStatusString);
        UserStatus GetUserStatusFromInt(in int userStatus);
    }
}