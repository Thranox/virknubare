using System.Threading.Tasks;
using Domain.Events;
using Domain.Interfaces;
using Domain.ValueObjects;
using Serilog;

namespace Infrastructure.DomainEvents.Handlers
{
    public class TravelExpenseChangedStateDomainEventHandler : IHandle<TravelExpenseChangedStateDomainEvent>
    {
        private readonly ILogger _logger;
        private readonly IMessageBrokerService _messageBrokerService;

        public TravelExpenseChangedStateDomainEventHandler(ILogger logger,
            IMessageBrokerService messageBrokerService)
        {
            _logger = logger;
            _messageBrokerService = messageBrokerService;
        }

        public async Task HandleAsync(TravelExpenseChangedStateDomainEvent travelExpenseChangedStateDomainEvent)
        {
            _logger.Information("Handling " + travelExpenseChangedStateDomainEvent.GetType().Name);

            // ------------------------------------------------------
            // Send message to those who can now process travel expense
            var usersAbleToProcessNewStage = travelExpenseChangedStateDomainEvent
                .TravelExpenseEntity
                .Customer
                .GetUsersAbleToProcess(travelExpenseChangedStateDomainEvent.TravelExpenseEntity.Stage);

            await _messageBrokerService
                .SendMessageAsync(
                    usersAbleToProcessNewStage,
                    travelExpenseChangedStateDomainEvent.TravelExpenseEntity,
                    MessageKind.YouCanNowProcessTravelExpense);

            // ------------------------------------------------------
            // Send message to owner that his/her travel expense has changed state
            // (But only if they didn't do it themselves)
            if (travelExpenseChangedStateDomainEvent.TravelExpenseEntity.OwnedByUser !=
                travelExpenseChangedStateDomainEvent.UserEntityMakingChange)
                await _messageBrokerService
                    .SendMessageAsync(
                        new[] {travelExpenseChangedStateDomainEvent.TravelExpenseEntity.OwnedByUser},
                        travelExpenseChangedStateDomainEvent.TravelExpenseEntity,
                        MessageKind.YouTravelExpenseHasChangedState);
        }
    }
}