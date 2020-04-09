using Domain.Interfaces;
using Domain.SharedKernel;
using Serilog;

namespace Infrastructure.DomainEvents.Handlers
{
    public class TravelExpenseUpdatedNotificationHandler : IHandle<TravelExpenseUpdatedDomainEvent>
    {
        private readonly ILogger _logger;
        private readonly IRepository _repository;

        public TravelExpenseUpdatedNotificationHandler(IRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public void Handle(TravelExpenseUpdatedDomainEvent travelExpenseUpdatedDomainEvent)
        {
            _logger.Information("Handling " + travelExpenseUpdatedDomainEvent.GetType().Name);
        }
    }
}