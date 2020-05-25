using Domain.Entities;
using Domain.SharedKernel;

namespace Domain.Events
{
    public class InvitationAddedDomainEvent : BaseDomainEvent
    {
        public CustomerEntity Customer { get; }
        public InvitationEntity Invitation { get; }

        public InvitationAddedDomainEvent(CustomerEntity customer, InvitationEntity invitation)
        {
            Customer = customer;
            Invitation = invitation;
        }
    }
}