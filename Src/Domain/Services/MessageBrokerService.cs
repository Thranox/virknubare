using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
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

        public async Task<int> SendMessageAsync(IEnumerable<UserEntity> userEntities,
            TravelExpenseEntity travelExpenseEntity, MessageKind messageKind)
        {
            var messagesSendCount = 0;
            var messageTemplate = _messageTemplateService.Get(messageKind);
            foreach (var userEntity in userEntities)
            {
                var messageValues = userEntity.GetMessageValues();
                var message = _messageFactory.GetMessage(messageTemplate, messageValues);
                foreach (var messageSenderService in _messageSenderServices)
                {
                    await messageSenderService.SendMessageAsync(message, userEntity);
                }
                messagesSendCount++;
            }

            return messagesSendCount;
        }
    }
}