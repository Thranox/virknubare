using System;
using System.IO;
using System.Threading.Tasks;
using Domain;
using Domain.Interfaces;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using Serilog;

namespace Infrastructure.Messaging
{
    public class EmailMessageSenderService : IMessageSenderService
    {
        private readonly ILogger _logger;

        public EmailMessageSenderService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task SendMessageAsync(IMessage message, IMessageReceiver messageReceiver)
        {
            var email = messageReceiver.Email;
            var filenameJson = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}{email}-{message.Subject}.json"
                .Replace("@", "-");
            var filenameHtml = $"_{email}-{message.Subject}.html"
                .Replace("@", "-");
            // TODO
            email = "andersjuulsfirma@gmail.com";

            // API Key "Improvento" under sendgrid.com account; jcianders  // 21Bananer
            var apiKey = "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";

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("test@example.com", "Example User");
            var subject = Globals.EmailPrefix + message.Subject;
            var to = new EmailAddress(email);
            var htmlContent = message.Body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);

            var json = JsonConvert.SerializeObject(msg, Formatting.Indented);
            _logger.Information("Sending email: {emailMessage}", json);

            var fullPathJson = Path.Combine(Path.GetTempPath(), filenameJson);
            File.WriteAllText(fullPathJson, json);

            var fullPathHtml = Path.Combine(Path.GetTempPath(), filenameHtml);
            File.WriteAllText(fullPathHtml, message.Body);

            //var response = await client.SendEmailAsync(msg);

            await Task.CompletedTask;
        }
    }
}