using System.Threading.Tasks;
using Domain.Interfaces;

namespace Infrastructure.Messaging
{
    public class EmailSenderService : IMessageSenderService
    {
        public async Task SendMessageAsync(IMessage message)
        {
            // TODO

            await Task.CompletedTask;
        }
    }
}