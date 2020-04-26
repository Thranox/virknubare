using System;
using Serilog.Core;
using Serilog.Events;

namespace API.Shared.ActionFilters
{
    public class TransactionIdEnricher : ILogEventEnricher
    {
        private readonly Guid _transactionId;

        public TransactionIdEnricher(Guid transactionId)
        {
            _transactionId = transactionId;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(
                propertyFactory.CreateProperty("TransactionId", _transactionId)
            );
        }
    }
}