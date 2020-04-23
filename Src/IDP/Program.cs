using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Humio;
using Serilog.Sinks.SystemConsole.Themes;
using SharedWouldBeNugets;

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
                    SetupConfig(args, config, hostingContext);
                    Log.Logger =StartupHelper.CreateLogger(config);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog();
                });
        }

        public static void SetupConfig(string[] args, IConfigurationBuilder config, HostBuilderContext hostingContext)
        {
            config.Sources.Clear();

            var env = hostingContext.HostingEnvironment;
            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            config.AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true, reloadOnChange: true);
            config.AddJsonFile($"appsettings.{env.EnvironmentName}.{Environment.MachineName}.json", optional: true,
                reloadOnChange: true);

            config.AddEnvironmentVariables();


            if (args != null)
            {
                config.AddCommandLine(args);
            }
        }
    }
}