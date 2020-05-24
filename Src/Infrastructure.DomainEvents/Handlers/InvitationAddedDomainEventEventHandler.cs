using System;
using System.Threading.Tasks;
using Domain.Events;
using Domain.Interfaces;
using Domain.ValueObjects;
using Serilog;

namespace Infrastructure.DomainEvents.Handlers
{
    public class InvitationAddedDomainEventEventHandler : IHandle<InvitationAddedDomainEvent>
    {
        private readonly ILogger _logger;
        private readonly IMessageBrokerService _messageBrokerService;

        public InvitationAddedDomainEventEventHandler(ILogger logger,
            IMessageBrokerService messageBrokerService)
        {
            _logger = logger;
            _messageBrokerService = messageBrokerService;
        }

        public async Task HandleAsync(InvitationAddedDomainEvent invitationAddedDomainEvent)
        {
            if(invitationAddedDomainEvent==null)
                throw new ArgumentNullException(nameof(invitationAddedDomainEvent));

            _logger.Information("Handling " + invitationAddedDomainEvent.GetType().Name);

            // ------------------------------------------------------
            // Send message to invitee

            await _messageBrokerService
                .SendEmailAsync(
                    invitationAddedDomainEvent.Invitation.Email,
                    MessageKind.YouHaveReceivedInvitation,
                    invitationAddedDomainEvent.Customer);
        }
    }
}