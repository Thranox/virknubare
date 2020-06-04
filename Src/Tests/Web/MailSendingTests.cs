using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using NUnit.Framework;
using SendGrid;
using SendGrid.Helpers.Mail;
using Application.Services;
using System.Runtime.InteropServices;

namespace Tests.Web
{
    public class MailSendingTests
    {
        [Test]
        [Explicit]
        public async Task SendEmail()
        {
            var subject = "Test";
            var body = "Test email from SMTP server";
            var fromAddress = "tobias.jensen@improvento.com";
            var toAddress = "tobias.jensen@improvento.com";

            var mailService = new MailService();
            await mailService.SendAsync(fromAddress, toAddress, subject, body);
        }
    }
}