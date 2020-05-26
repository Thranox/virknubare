using System;
using System.Collections.Generic;
using Domain.SharedKernel;

namespace Application.Dtos
{
    public class UserPermissionDto : ValueObject
    {
        public Guid UserId { get; set; }
        public int UserStatus { get; set; }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield break;
        }
    }
}