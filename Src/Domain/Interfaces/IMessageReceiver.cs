namespace Domain.Interfaces
{
    public interface IMessageReceiver:IMessageValueEnricher
    {
        string Email { get; }
    }
}