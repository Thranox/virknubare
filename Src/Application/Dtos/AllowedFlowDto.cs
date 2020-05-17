using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Application.Dtos
{
    public class AllowedFlowDto:ValueObject
    {
        public Guid FlowStepId { get; set; }
        public string Description { get; set; }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Description;
        }
    }
}