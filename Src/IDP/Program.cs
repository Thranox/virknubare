using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Humio;
using Serilog.Sinks.SystemConsole.Themes;

namespace IDP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.Sources.Clear();

                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{env.EnvironmentName}.{Environment.MachineName}.json", optional: true, reloadOnChange: true);

                    config.AddEnvironmentVariables();

                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .MinimumLevel.Override("System", LogEventLevel.Warning)
                        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                        .WriteTo.HumioSink(new HumioSinkConfiguration
                        {
                            BatchSizeLimit = 50,
                            Period = TimeSpan.FromSeconds(5),
                            Tags = new KeyValuePair<string, string>[]{
                                new KeyValuePair<string, string>("source", "ApplicationLog"),
                                new KeyValuePair<string, string>("environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                            },
                            IngestToken = Environment.GetEnvironmentVariable("HUMIO_KEY") 
                        })
                        .Enrich.WithMachineName()
                        //.Enrich.WithProperty("ReleaseNumber", settings.ReleaseNumber)
                        .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                        .Enrich.WithProperty("ComponentName", "IDP")
                        .Enrich.FromLogContext()
                        .ReadFrom.Configuration(config.Build())
                        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                        .CreateLogger();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog();
                });
        }
    }
}