using Application.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MailService : IMailService
    {
        public async Task SendAsync(string fromAddress, string toAddress, string subject, string body)
        {
            var smtp = new SmtpClient();
            {
                smtp.Host = "172.20.157.15";
                smtp.Port = 25;
                smtp.EnableSsl = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential("", "");
                smtp.Timeout = 100000;
            }
            await smtp.SendMailAsync(fromAddress, toAddress, subject, body);
        }
    }
}
