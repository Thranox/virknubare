using System;
using Microsoft.Extensions.Hosting;

namespace PolAPI
{
    public interface IMailSenderHostedService: IHostedService, IDisposable
    {
    }
}