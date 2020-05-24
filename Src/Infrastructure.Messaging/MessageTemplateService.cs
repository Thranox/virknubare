using System;
using Domain;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Infrastructure.Messaging
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
                        Body = $"Hej {KeyMessagesConst.UserName},<br/>" +
                               $"En rejseafregning er klar til behandling på https://wwww.politikerafregning.dk/stuff?id=abc"
                    };
                case MessageKind.YourTravelExpenseHasChangedState:
                    return new MessageTemplate()
                    {
                        Subject = "Din rejseafregning har skiftet tilstand",
                        Body = $"Hej {KeyMessagesConst.UserName},<br/>" +
                               $"Din rejseafregning er behandlet af en medarbejder.<br/>" +
                               $"Følg den på https://wwww.politikerafregning.dk/stuff?id=abc"
                    };
                case MessageKind.YouHaveReceivedInvitation:
                    return new MessageTemplate()
                    {
                        Subject = "Invitation til politikerafregning.dk",
                        Body = $"Hej, \n{KeyMessagesConst.CustomerName} har inviteret dig til at benytte Politikerafregning.dk.<br/>" +
                               $"Åbn nedenstående link for at oprette dig som bruger.<br/>" +
                               $"Bemærk, at invitationen skal aktiveres indenfor 14 dage.<br/>" +
                               $"https://wwww.politikerafregning.dk/stuff?id=abc"
                    };
                default:
                    throw new NotImplementedException();
            }
        }
    }
}