using System.Collections.Generic;
using Domain.SharedKernel;

namespace Application.Dtos
{
    public class CustomerUserGetResponse:ValueObject
    {
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield break;
        }

        public UserPermissionDto[] Users { get; set; }
    }
}