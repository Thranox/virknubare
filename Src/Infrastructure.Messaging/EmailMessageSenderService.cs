using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Messaging
{
    public class EmailMessageSenderService : IMessageSenderService
    {
        private readonly IEmailSenderService _emailSenderService;

        public EmailMessageSenderService(IEmailSenderService emailSenderService)
        {
            _emailSenderService = emailSenderService;
        }

        public async Task SendMessageAsync(IMessage message, UserEntity userEntity)
        {
            await _emailSenderService.SendEmailAsync(message, userEntity.Email);
        }
    }
}