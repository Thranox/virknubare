using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services
{
    public class MessageBrokerService : IMessageBrokerService
    {
        private readonly IEnumerable<IMessageSenderService> _messageSenderServices;

        public MessageBrokerService(IEnumerable<IMessageSenderService> messageSenderServices)
        {
            _messageSenderServices = messageSenderServices;
        }

        public async Task SendWelcomeMessageAsync(UserEntity user)
        {
            var tasks = _messageSenderServices.Select(x => x.SendWelcomeMessageAsync(user));
            await Task.WhenAll(tasks);
        }
    }
}