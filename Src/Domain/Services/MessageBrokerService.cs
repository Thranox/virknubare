using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Services
{
    public class MessageBrokerService : IMessageBrokerService
    {
        private readonly IMessageFactory _messageFactory;
        private readonly IEnumerable<IMessageSenderService> _messageSenderServices;
        private readonly IMessageTemplateService _messageTemplateService;

        public MessageBrokerService(IEnumerable<IMessageSenderService> messageSenderServices,
            IMessageTemplateService messageTemplateService, IMessageFactory messageFactory)
        {
            _messageSenderServices = messageSenderServices;
            _messageTemplateService = messageTemplateService;
            _messageFactory = messageFactory;
        }

        public async Task<int> SendMessageAsync(IEnumerable<IMessageReceiver> messageReceivers, MessageKind messageKind,
            IMessageValueEnricher[] messageValueEnrichers)
        {
            var messagesSendCount = 0;
            var messageTemplate = _messageTemplateService.Get(messageKind);

            foreach (var messageReceiver in messageReceivers)
            {
                // Get the values that can be substituted for placeholders in template
                var messageValues = new Dictionary<string, string>();
                var enrichers = new List<IMessageValueEnricher>(messageValueEnrichers) {messageReceiver};
                foreach (var enricher in enrichers) enricher.Enrich(messageValues);

                // Get the message as copy of template but with all placeholders substitued for values.
                var message = _messageFactory.GetMessage(messageTemplate, messageValues);

                // With the message in hand, send it using all available senders.
                foreach (var messageSenderService in _messageSenderServices)
                    await messageSenderService.SendMessageAsync(message, messageReceiver);

                messagesSendCount++;
            }

            return messagesSendCount;
        }
    }
}