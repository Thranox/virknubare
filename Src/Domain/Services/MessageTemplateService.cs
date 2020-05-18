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
                        Subject = "Rejseafregning klar til behandling",
                        Body = ""
                    };
                case MessageKind.YourTravelExpenseHasChangedState:
                    return new MessageTemplate()
                    {
                        Subject = "Din rejseafregning har skiftet tilstand",
                        Body = ""
                    };
                default:
                    throw new NotImplementedException();
            }
        }
    }
}