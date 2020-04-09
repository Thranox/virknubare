using System;
using Domain.SharedKernel;

namespace Web.ApiModels
{
    public class BusinessRuleViolationResponseDto : ValueObject
    {
        public Guid EntityId { get; set; }
        public string Message { get; set; }
    }
}