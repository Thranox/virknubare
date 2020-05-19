using System;
using AutoFixture;
using Serilog;
using Serilog.Core;

namespace Tests.TestHelpers
{
    public abstract class BaseTestContext:IDisposable
    {
        public BaseTestContext()
        {
            Log.Logger= new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            Log.Logger.Information("Starting test");

            Fixture = new Fixture();
        }

        public Fixture Fixture { get; }

        public void Dispose()
        {
        }
    }
}