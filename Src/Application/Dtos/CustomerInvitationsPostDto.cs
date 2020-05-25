using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Application.Dtos
{
    public class CustomerInvitationsPostDto : ValueObject
    {
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield break;
        }

        public string[] Emails { get; set; }
    }
}