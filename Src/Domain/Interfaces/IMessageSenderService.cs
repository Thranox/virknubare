using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMessageSenderService
    {
        Task SendMessageAsync(IMessage message, UserEntity userEntity);
    }
}