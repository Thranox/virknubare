using System.Threading.Tasks;

namespace SharedWouldBeNugets
{
    public interface IMailService
    {
        Task SendAsync(string fromAddress, string toAddress, string subject, string body);
    }
}
