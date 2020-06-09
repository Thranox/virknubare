using System;
using System.Collections.Generic;
using Domain.SharedKernel;

namespace Application.Dtos
{
    public class UserCustomerPostDto:ValueObject
    {
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return CustomerId;
        }

        public Guid CustomerId { get; set; }
    }
}