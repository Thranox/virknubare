using System.Collections.Generic;
using Domain.SharedKernel;

namespace Application.Dtos
{
    public class UserCustomerPutResponse:ValueObject
    {
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield break;
        }
    }
}