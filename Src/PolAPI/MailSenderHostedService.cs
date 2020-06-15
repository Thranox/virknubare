using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Specifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SharedWouldBeNugets;

namespace PolAPI
{
    public class MailSenderHostedService : BackgroundService, IMailSenderHostedService
    {
        private readonly string _fromAddress;
        private readonly int _intervalInSeconds;
        private readonly ILogger _logger;
        private readonly IMailService _mailService;
        private readonly IServiceProvider _serviceProvider;

        public MailSenderHostedService(ILogger logger, IServiceProvider serviceProvider, IMailService mailService,
            string fromAddress, int intervalInSeconds)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _mailService = mailService;
            _fromAddress = fromAddress;
            _intervalInSeconds = intervalInSeconds;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information(GetType().FullName + " starting up.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.Verbose("Checking for mails to send.");
                using var serviceScope = _serviceProvider
                    .CreateScope();
                using var unitOfWork = serviceScope
                    .ServiceProvider
                    .GetRequiredService<IUnitOfWork>();

                var emailEntities = unitOfWork.Repository.List(new WaitingEmails()).Take(2).ToArray();
                var emailEntity = emailEntities.FirstOrDefault();
                if (emailEntity == null)
                {
                    // Nothing to send now. Wait then try again.
                    await Task.Delay(TimeSpan.FromSeconds(_intervalInSeconds), stoppingToken);
                    continue;
                }

                try
                {
                    _logger.Information(GetType().FullName + " Sending email.");
                    
                    await _mailService
                        .SendAsync(_fromAddress, emailEntity.Recievers, emailEntity.Subject,emailEntity.Body);
                    
                    emailEntity.SendTime = DateTime.Now;

                    await unitOfWork.CommitAsync();
                }
                catch (Exception e)
                {
                    _logger.Error(e, "During MailSenderHostedService send attempt.");
                }
            }

            _logger.Information(GetType().FullName + " shutting down.");
        }
    }
}