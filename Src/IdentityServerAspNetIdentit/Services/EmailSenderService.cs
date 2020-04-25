using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IDP.Services
{
    public class EmailSenderService : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Note: This API Key for sendgrid is for dev & test only. If we use sendgrid for production we need an improvento account.
            var client = new SendGridClient("SG" +
                                            "." +
                                            "yNWsGff6QIK9Ob1hQ0_EGw" +
                                            "." +
                                            "TqTZWHYI1cb6dktFe3S19FdA8" +
                                            "Y" +
                                            "NLjPsyQixXY_37VAY");

            // Send using the Mail Helper with convenience methods and initialized SendGridMessage object
            var msg = new SendGridMessage
            {
                From = new EmailAddress("andersjuulsfirma@gmail.com", "Anders Juul"), // Eh, what to put here?!
                Subject = subject,
                HtmlContent = htmlMessage
            };
            msg.AddTo(new EmailAddress(email));

            await client.SendEmailAsync(msg);
        }
    }
}