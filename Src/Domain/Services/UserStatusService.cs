using System;
using System.Collections.Generic;
using Domain.ValueObjects;

namespace Domain.Services
{
    public class UserStatusService : IUserStatusService
    {
        private readonly Dictionary<string, UserStatus> _dictionary;

        public UserStatusService()
        {
            _dictionary = new Dictionary<string, UserStatus>();
            foreach (UserStatus userStatus in Enum.GetValues(typeof(UserStatus)))
                _dictionary.Add(userStatus.ToString(), userStatus);
        }

        public UserStatus GetUserStatusFromString(string userStatusString)
        {
            if (_dictionary.ContainsKey(userStatusString))
                return _dictionary[userStatusString];

            throw new ArgumentException(nameof(userStatusString));
        }
    }
}