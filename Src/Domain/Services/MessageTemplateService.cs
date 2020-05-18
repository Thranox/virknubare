using System;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Services
{
    public class MessageTemplateService : IMessageTemplateService
    {
        public IMessageTemplate Get(MessageKind messageKind)
        {
            switch (messageKind)
            {
                case MessageKind.YouCanNowProcessTravelExpense:
                    return new MessageTemplate()
                    {
                        Subject = "",
                        Body=""
                    };
                default:
                    throw new NotImplementedException();
            }
        }
    }
}