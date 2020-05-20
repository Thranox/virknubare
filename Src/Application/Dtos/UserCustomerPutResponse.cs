using System.Collections.Generic;
using CSharpFunctionalExtensions;

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