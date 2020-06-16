using System.Threading.Tasks;
using NUnit.Framework;
using Serilog;
using SharedWouldBeNugets;

namespace Tests.SharedWouldBeNugets
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

            var mailService = new MailService(Log.Logger, "188.244.78.162",3000);
            await mailService.SendAsync(fromAddress,new []{ toAddress}, subject, body);
        }
    }
}