using System;
using System.Linq;

namespace Domain.Entities
{
    public class PayoutTableRow
    {
        private PayoutTableRow(string[][] strings)
        {
            Items = strings;
        }

        public string[][] Items { get;  }

        public static PayoutTableRow Create(string[] strings)
        {
            return new PayoutTableRow(strings.Select(x=> new String []{x}).ToArray());
        }

        public static PayoutTableRow Create(string[][] strings)
        {
            return new PayoutTableRow(strings);
        }
    }
}