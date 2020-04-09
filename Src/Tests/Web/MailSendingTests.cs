using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Tests.Web
{
    public class MailSendingTests
    {
        [Test]
        [Explicit]
        public async Task SendEmail()
        {
            // Note: This API Key for sendgrid is for dev & test only. If we use sendgrid for production we need an improvento account.
            var client = new SendGridClient("SG"+
                                            "."+
                                            "yNWsGff6QIK9Ob1hQ0_EGw"+
                                            "."+
                                            "TqTZWHYI1cb6dktFe3S19FdA8"+ 
                                            "Y" + 
                                            "NLjPsyQixXY_37VAY");

            // Send a Single Email using the Mail Helper
            var from = new EmailAddress("andersjuulsfirma@gmail.com", "Anders Juul");
            var subject = "Hello World from the Twilio SendGrid CSharp Library Helper!";
            var to = new EmailAddress("andersjuulsfirma@gmail.com", "Anders Juul");
            var plainTextContent = "Hello, Email from the helper [SendSingleEmailAsync]!";
            var htmlContent = "<strong>Hello, Email from the helper! [SendSingleEmailAsync]</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            Response response = await client.SendEmailAsync(msg);
            Console.WriteLine(msg.Serialize());
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Headers);

            // Send a Single Email using the Mail Helper with convenience methods and initialized SendGridMessage object
            msg = new SendGridMessage()
            {
                From = new EmailAddress("andersjuulsfirma@gmail.com", "Anders Juul"),
                Subject = "Hello World 22 from the Twilio SendGrid CSharp Library Helper!",
                PlainTextContent = "Hello, Email from the helper [SendSingleEmailAsync]!",
                HtmlContent = "<strong>Hello, Email from the helper! [SendSingleEmailAsync]</strong>"
            };
            msg.AddTo(new EmailAddress("andersjuulsfirma@gmail.com", "Anders Juul"));

            response = await client.SendEmailAsync(msg);
            Console.WriteLine(msg.Serialize());
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Headers);
            Console.WriteLine("\n\nPress <Enter> to continue.");
        }
    }
}