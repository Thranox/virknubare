using Domain.Interfaces;

namespace Domain.ValueObjects
{
    public class Message : IMessage
    {
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}