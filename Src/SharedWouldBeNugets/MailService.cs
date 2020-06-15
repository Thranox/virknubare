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

        public MailService(ILogger logger)
        {
            _logger = logger;
        }
        public async Task SendAsync(string fromAddress, string toAddress, string subject, string body)
        {
            try
            {
                var smtp = new SmtpClient();
                {
                    smtp.Host = "188.244.78.162";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential("", "");
                    smtp.Timeout = 100000;
                }
                await smtp.SendMailAsync(fromAddress, toAddress, subject, body);
            }
            catch (Exception e)
            {
                _logger.Error(e,"During sending of email");
                throw;
            }
        }
    }
}