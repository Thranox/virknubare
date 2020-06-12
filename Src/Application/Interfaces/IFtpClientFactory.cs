using Domain.Entities;
using FluentFTP;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IFtpClientFactory
    {
        IFtpClient Create(CustomerEntity customerEntity);
    }
}
