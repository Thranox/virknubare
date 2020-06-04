using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    interface IMailService
    {
        Task SendAsync(string fromAddress, string toAddress, string subject, string body);
    }
}
