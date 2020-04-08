using System;
using System.Collections.Generic;
using System.Text;
using Domain.Interfaces;
using Domain.SharedKernel;
using Serilog;

namespace Infrastructure.DomainEvents.Handlers
{
    public class TravelExpenseUpdatedNotificationHandler : IHandle<TravelExpenseUpdatedDomainEvent>
    {
        private readonly IRepository _repository;
        private readonly ILogger _logger;

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
