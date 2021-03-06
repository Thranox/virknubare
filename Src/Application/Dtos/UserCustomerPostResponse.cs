﻿using System;
using System.Collections.Generic;
using Domain.SharedKernel;

namespace Application.Dtos
{
    public class UserCustomerPostResponse : ValueObject
    {
        public IEnumerable<Guid> Ids { get; set; }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield break;
        }
    }
}