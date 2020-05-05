using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Humio;
using Serilog.Sinks.SystemConsole.Themes;

namespace SharedWouldBeNugets
{
    public static class StartupHelper
    {
        public static Logger CreateLogger(IConfigurationBuilder config, string componentName)
        {
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .WriteTo.HumioSink(new HumioSinkConfiguration
                {
                    BatchSizeLimit = 50,
                    Period = TimeSpan.FromSeconds(5),
                    Tags = new KeyValuePair<string, string>[]{
                        new KeyValuePair<string, string>("source", "Politikerafregning"),
                        new KeyValuePair<string, string>("environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                    },
                    IngestToken = Environment.GetEnvironmentVariable("HUMIO_KEY") 
                })
                .Enrich.WithMachineName()
                //.Enrich.WithProperty("ReleaseNumber", settings.ReleaseNumber)
                .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                .Enrich.WithProperty("SuiteName", "Pol")
                .Enrich.WithProperty("ComponentName", componentName)
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(config.Build())
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);
            var logger = loggerConfiguration.CreateLogger();
            logger.Information("---------------------------");
            logger.Information("Application started");
            return logger;
        }

        public static void SetupConfig(string[] args, IConfigurationBuilder config, string environmentName)
        {
            config.Sources.Clear();

            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            config.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);
            config.AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true, reloadOnChange: true);
            config.AddJsonFile($"appsettings.{environmentName}.{Environment.MachineName}.json", optional: true,
                reloadOnChange: true);

            config.AddEnvironmentVariables();


            if (args != null)
            {
                config.AddCommandLine(args);
            }
        }
    }
}
