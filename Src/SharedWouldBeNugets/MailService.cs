using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Serilog;

namespace SharedWouldBeNugets
{
    public class MailService : IMailService
    {
        private readonly ILogger _logger;
        private readonly string _smtpIp;
        private readonly int _smtpTimeoutMs;

        public MailService(ILogger logger, string smtpIp, int smtpTimeoutMs)
        {
            _logger = logger;
            _smtpIp = smtpIp;
            _smtpTimeoutMs = smtpTimeoutMs;
        }

        public async Task SendAsync(string fromAddress, string toAddress, string subject, string body)
        {
            try
            {
                var smtp = new SmtpClient();
                {
                    smtp.Host = _smtpIp;
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential("", "");
                    smtp.Timeout = _smtpTimeoutMs;
                }
                await smtp.SendMailAsync(fromAddress, toAddress, subject, body);
            }
            catch (Exception e)
            {
                _logger.Error(e, "During sending of email");
                throw;
            }
        }
    }
}