using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IMessageFactory
    {
        IMessage GetMessage(IMessageTemplate messageTemplate, Dictionary<string, string> messageValues);
    }
}