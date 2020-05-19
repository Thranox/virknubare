namespace Domain.Interfaces
{
    public interface IMessage
    {
        string Subject { get; set; }
        string Body { get; set; }
    }
}