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
        private readonly IEmailSenderService _emailSenderService;
        private readonly IEnumerable<IMessageSenderService> _messageSenderServices;
        private readonly IMessageTemplateService _messageTemplateService;

        public MessageBrokerService(IEnumerable<IMessageSenderService> messageSenderServices,
            IMessageTemplateService messageTemplateService, IMessageFactory messageFactory, IEmailSenderService emailSenderService)
        {
            _messageSenderServices = messageSenderServices;
            _messageTemplateService = messageTemplateService;
            _messageFactory = messageFactory;
            _emailSenderService = emailSenderService;
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

        public async Task SendEmailAsync(string email, MessageKind messageKind, CustomerEntity customer)
        {
            var messageTemplate = _messageTemplateService.Get(messageKind);
            var message = _messageFactory.GetMessage(messageTemplate, customer.GetValues());

            await _emailSenderService.SendEmailAsync(message, email);
        }
    }
}