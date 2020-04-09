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
            Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            Logger.Information("Starting test");

            Fixture = new Fixture();
        }

        public Fixture Fixture { get; }
        public Logger Logger { get; private set; }

        public void Dispose()
        {
            Logger?.Dispose();
        }
    }
}