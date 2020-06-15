using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace PolAPI.BackgroundServices
{
    public class WeatherCacheService: BackgroundService,IWeatherCacheService
    {
        private readonly TimeSpan _pollingInterval;

        public WeatherCacheService(TimeSpan pollingInterval)
        {
            _pollingInterval = pollingInterval;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }

    public interface IWeatherCacheService
    {
    }
}
