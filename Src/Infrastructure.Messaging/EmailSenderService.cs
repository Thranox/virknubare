using System;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Infrastructure.Messaging
{
    public class EmailSenderService : IMessageSenderService
    {
        public Task SendMessageAsync(IMessage message)
        {
            throw new NotImplementedException();
        }
    }
}