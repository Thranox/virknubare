using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Messaging
{
    public class EmailSenderService : IMessageSenderService
    {
        public async Task SendWelcomeMessageAsync(UserEntity user)
        {
            throw new NotImplementedException();
        }
    }
}
