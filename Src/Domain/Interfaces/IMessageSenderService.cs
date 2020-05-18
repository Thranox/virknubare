using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IMessageSenderService
    {
        Task SendMessageAsync(IMessage message);
    }
}