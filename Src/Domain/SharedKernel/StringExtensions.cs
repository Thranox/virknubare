using System.Collections.Generic;

namespace Domain.SharedKernel
{
    public static class StringExtensions
    {
        public static string Replace(this string s, Dictionary<string, string> values)
        {
            foreach (var messageValue in values)
            {
                s= s.Replace(messageValue.Key, messageValue.Value);
            }

            return s;
        }
    }
}