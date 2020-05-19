﻿using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Messaging
{
    public class EmailMessageSenderService : IMessageSenderService
    {
        public async Task SendMessageAsync(IMessage message, UserEntity userEntity)
        {
            // TODO

            await Task.CompletedTask;
        }
    }
}