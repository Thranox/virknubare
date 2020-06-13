using System.IO;
using System.Threading.Tasks;
using SharedWouldBeNugets;

namespace TestHelpers
{
    public class FakeMailService : IMailService
    {
        public Task SendAsync(string fromAddress, string toAddress, string subject, string body)
        {
            var tempFileName = Path.GetTempFileName()+".txt";
            File.WriteAllLines(tempFileName, new[]{fromAddress,toAddress,subject,body});

            return Task.CompletedTask;
        }
    }
}