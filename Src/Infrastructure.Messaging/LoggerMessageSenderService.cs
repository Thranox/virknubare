using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Newtonsoft.Json;
using Serilog;

namespace Infrastructure.Messaging
{
    public class LoggerMessageSenderService : IMessageSenderService
    {
        private readonly ILogger _logger;

        public LoggerMessageSenderService(ILogger logger)
        {
            _logger = logger;
        }

        public Task SendMessageAsync(IMessage message, UserEntity userEntity)
        {
            _logger.Information("Sending message : " + JsonConvert.SerializeObject(message) + " to " + userEntity.Name +
                                "/" + userEntity.Subject);
            return Task.CompletedTask;
        }
    }
}