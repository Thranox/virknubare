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
                        Body = "Hej $$firstname$$, en rejseafregning er klar til behandling på https://wwww.politikerafregning.dk/stuff?id=abc"
                    };
                case MessageKind.YourTravelExpenseHasChangedState:
                    return new MessageTemplate()
                    {
                        Subject = "Din rejseafregning har skiftet tilstand",
                        Body = "Hej $$firstname$$, din rejseafregning er behandlet af en medarbejder. Følg den på https://wwww.politikerafregning.dk/stuff?id=abc"
                    };
                default:
                    throw new NotImplementedException();
            }
        }
    }
}