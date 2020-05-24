using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(IMessage message, string email);
    }
}