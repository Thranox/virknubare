using System.Collections.Generic;
using Domain.SharedKernel;

namespace Application.Dtos
{
    public class DailyAllowanceAmountDto:ValueObject
    {
        public int DaysLessThan4hours { get; set; }
        public int DaysMoreThan4hours { get; set; }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return DaysLessThan4hours;
            yield return DaysMoreThan4hours;
        }
    }
}