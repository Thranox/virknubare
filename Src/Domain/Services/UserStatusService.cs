using System;
using System.Collections.Generic;
using Domain.ValueObjects;

namespace Domain.Services
{
    public class UserStatusService : IUserStatusService
    {
        private readonly Dictionary<int, UserStatus> _intToEnumDictionary;
        private readonly Dictionary<string, UserStatus> _stringToEnumDictionary;

        public UserStatusService()
        {
            _stringToEnumDictionary = new Dictionary<string, UserStatus>();
            _intToEnumDictionary = new Dictionary<int, UserStatus>();
            foreach (UserStatus userStatus in Enum.GetValues(typeof(UserStatus)))
            {
                _stringToEnumDictionary.Add(userStatus.ToString(), userStatus);
                _intToEnumDictionary.Add((int) userStatus, userStatus);
            }
        }

        public UserStatus GetUserStatusFromString(string userStatusString)
        {
            if (_stringToEnumDictionary.ContainsKey(userStatusString))
                return _stringToEnumDictionary[userStatusString];

            throw new ArgumentException(nameof(userStatusString));
        }

        public UserStatus GetUserStatusFromInt(in int userStatus)
        {
            if (_intToEnumDictionary.ContainsKey(userStatus))
                return _intToEnumDictionary[userStatus];

            throw new ArgumentException(nameof(userStatus));
        }
    }
}