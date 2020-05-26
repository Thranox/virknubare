using System.Collections.Generic;
using Domain.SharedKernel;

namespace Application.Dtos
{
    public class StatisticsGetResponse : ValueObject
    {
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield break;
        }
    }
}