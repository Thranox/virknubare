using System.Threading.Tasks;
using Domain.Interfaces;
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

        public Task SendMessageAsync(IMessage message)
        {
            _logger.Information("Sending message : " + message);
            return Task.CompletedTask;
        }
    }
}