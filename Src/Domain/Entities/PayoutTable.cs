using System;
using System.Collections.Generic;
using Domain.SharedKernel;

namespace Domain.Entities
{
    public class PayoutTable:ValueObject
    {
        public override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}