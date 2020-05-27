using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Application.Dtos
{
    public class CustomerInvitationsPostResponse:ValueObject
    {
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield break;
        }
    }
}