using System.Collections.Generic;
using CSharpFunctionalExtensions;

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