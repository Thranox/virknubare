using System;
using System.Threading.Tasks;
using Domain.Entities;
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

        public Task SendWelcomeMessageAsync(UserEntity user)
        {
            _logger.Information("Sending welcome message to user: " + user);
            return Task.CompletedTask;
        }
    }
}