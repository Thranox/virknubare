using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Domain.SharedKernel;

namespace Application.Dtos
{
    public class SubmissionPostResponse : ValueObject
    {
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield break;
        }
    }
}