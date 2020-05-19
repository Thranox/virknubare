using Domain.Interfaces;

namespace Domain.ValueObjects
{
    public class MessageTemplate : IMessageTemplate
    {
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}