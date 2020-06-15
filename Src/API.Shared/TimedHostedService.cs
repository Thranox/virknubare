using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Specifications;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SharedWouldBeNugets;

namespace PolAPI
{
    public class TimedHostedService : ITimedHostedService
    {
        private int executionCount = 0;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMailService _mailService;
        private readonly string _fromAddress;
        private Timer _timer;

        public TimedHostedService(ILogger logger, IServiceProvider serviceProvider, IMailService mailService, string fromAddress)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _mailService = mailService;
            _fromAddress = fromAddress;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.Information(
                "Timed Hosted Service is working. Count: {Count}", count);

            // Create uow, 
            using var serviceScope = _serviceProvider
                .CreateScope();
            using var unitOfWork=serviceScope
                .ServiceProvider
                .GetRequiredService<IUnitOfWork>();

            var emailEntity = unitOfWork.Repository.List(new WaitingEmails()).FirstOrDefault();
            if(emailEntity==null)
                return;

            try
            {
                _mailService.SendAsync(_fromAddress, emailEntity.Recievers, emailEntity.Subject, emailEntity.Body).Wait(TimeSpan.FromSeconds(5));
                emailEntity.SendTime=DateTime.Now;

                unitOfWork.CommitAsync().Wait();
            }
            catch (Exception e)
            {
                _logger.Error(e, "During MailService send attempt.");
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}