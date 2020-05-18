using System.Collections.Generic;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Services
{
    public class MessageFactory : IMessageFactory
    {
        public IMessage GetMessage(IMessageTemplate messageTemplate, Dictionary<string, string> messageValues)
        {
            return new Message();
        }
    }
}