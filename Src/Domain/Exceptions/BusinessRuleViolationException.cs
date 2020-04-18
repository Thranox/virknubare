using System;

namespace Domain.Exceptions
{
    public class BusinessRuleViolationException:Exception
    {
        public Guid EntityId { get; }

        public BusinessRuleViolationException(Guid entityId, string message):base(message)
        {
            EntityId = entityId;
        }
    }
}