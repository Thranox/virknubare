using System.Threading.Tasks;
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

        public Task SendMessageAsync(IMessage message, IMessageReceiver messageReceiver)
        {
            _logger.Information("Sending message : " + JsonConvert.SerializeObject(message) + " to " + messageReceiver.Email );
            return Task.CompletedTask;
        }
    }
}