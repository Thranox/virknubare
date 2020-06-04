using System.Threading.Tasks;

namespace Application.Interfaces
{
    interface IMailService
    {
        Task SendAsync(string fromAddress, string toAddress, string subject, string body);
    }
}
