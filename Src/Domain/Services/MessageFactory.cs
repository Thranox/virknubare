using System.Collections.Generic;
using Domain.Interfaces;

namespace Domain.Services
{
    public class MessageFactory : IMessageFactory
    {
        public IMessage GetMessage(IMessageTemplate messageTemplate, Dictionary<string, string> messageValues)
        {
            throw new System.NotImplementedException();
        }
    }
}