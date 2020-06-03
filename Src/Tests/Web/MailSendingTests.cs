using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using NUnit.Framework;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Tests.Web
{
    public class MailSendingTests
    {
        [Test]
        public async Task SendEmail()
        {

            var smtp = new SmtpClient();
            {
                smtp.Host = "172.20.157.15";
                smtp.Port = 25;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential("", "");
                smtp.Timeout = 100000;
            }
            var subject = "Test";
            var body = "Test email from SMTP server";
            var fromAddress = "tobias.jensen@improvento.com";
            var toAddress = "tobias.jensen@improvento.com";
            await smtp.SendMailAsync(fromAddress, toAddress, subject, body);
        }
    }
}