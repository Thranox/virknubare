using System;
using Microsoft.Extensions.Hosting;

namespace PolAPI
{
    public interface ITimedHostedService: IHostedService, IDisposable
    {
    }
}