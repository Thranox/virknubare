namespace Domain.Interfaces
{
    public interface IMessageSender
    {
        void SendGuestbookNotificationEmail(string toAddress, string messageBody);
    }
}