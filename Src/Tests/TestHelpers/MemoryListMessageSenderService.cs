using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Tests.TestHelpers
{
    public class MemoryListMessageSenderService:IMessageSenderService
    {
        public readonly Dictionary<DateTime,object[]> MessagesSend = new Dictionary<DateTime, object[]>();

        public async Task SendMessageAsync(IMessage message, IMessageReceiver messageReceiver)
        {
            MessagesSend.Add(DateTime.Now, new object[] { message, messageReceiver });
            await Task.CompletedTask;
        }
    }
}