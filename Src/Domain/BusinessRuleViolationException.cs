using System;

namespace Domain
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