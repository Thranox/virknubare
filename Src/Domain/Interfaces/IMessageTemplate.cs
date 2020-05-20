namespace Domain.Interfaces
{
    public interface IMessageTemplate
    {
        string Subject { get; set; }
        string Body { get; set; }
    }
}