using Domain.Entities;
using Domain.Interfaces;
using Domain.SharedKernel;

namespace Domain.Events
{
    public class InvitationAddedDomainEvent : BaseDomainEvent
    {
        public CustomerEntity Customer { get; }
        public InvitationEntity Invitation { get; }
        public IMessageValueEnricher[] MessageValueEnrichers { get; }

        public InvitationAddedDomainEvent(CustomerEntity customer, InvitationEntity invitation,
            IMessageValueEnricher[] messageValueEnrichers)
        {
            Customer = customer;
            Invitation = invitation;
            MessageValueEnrichers = messageValueEnrichers;
        }
    }
}