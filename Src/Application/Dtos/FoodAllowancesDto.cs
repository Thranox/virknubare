using System.Collections.Generic;
using Domain.SharedKernel;

namespace Application.Dtos
{
    public class FoodAllowancesDto : ValueObject
    {
        public int Morning { get; set; }
        public int Lunch { get; set; }
        public int Dinner { get; set; }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Morning;
            yield return Lunch;
            yield return Dinner;
        }
    }
}