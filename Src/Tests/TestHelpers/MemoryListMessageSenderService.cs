using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Tests.TestHelpers
{
    public class MemoryListMessageSenderService:IMessageSenderService
    {
        public readonly Dictionary<DateTime,object[]> MessagesSend = new Dictionary<DateTime, object[]>();
        public async Task SendMessageAsync(IMessage message, UserEntity userEntity)
        {
            MessagesSend.Add(DateTime.Now, new object[]{message,userEntity});
            await Task.CompletedTask;
        }
    }
}