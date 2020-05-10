using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMessageSenderService
    {
        Task SendWelcomeMessageAsync(UserEntity user);
    }
}