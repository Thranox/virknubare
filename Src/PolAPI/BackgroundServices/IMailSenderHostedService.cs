using System;
using Microsoft.Extensions.Hosting;

namespace PolAPI.BackgroundServices
{
    public interface IMailSenderHostedService: IHostedService, IDisposable
    {
    }
}