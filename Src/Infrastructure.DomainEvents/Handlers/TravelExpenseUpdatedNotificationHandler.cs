using System.Threading.Tasks;
using Domain.Events;
using Domain.Interfaces;
using Domain.SharedKernel;
using Serilog;

namespace Infrastructure.DomainEvents.Handlers
{
    public class TravelExpenseUpdatedNotificationHandler : IHandle<TravelExpenseUpdatedDomainEvent>
    {
        private readonly ILogger _logger;

        public TravelExpenseUpdatedNotificationHandler(ILogger logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(TravelExpenseUpdatedDomainEvent travelExpenseChangedStateDomainEvent)
        {
            _logger.Information("Handling " + travelExpenseChangedStateDomainEvent.GetType().Name);

            await Task.CompletedTask;
        }
    }
}