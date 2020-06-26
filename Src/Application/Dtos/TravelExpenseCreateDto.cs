using System;
using System.Collections.Generic;
using Domain.SharedKernel;

namespace Application.Dtos
{
    public class TravelExpenseCreateDto : ValueObject
    {
        public string Description { get; set; }
        public Guid CustomerId { get; set; }
        
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Description;
            yield return CustomerId;
        }
    }
}