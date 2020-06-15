using API.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SharedWouldBeNugets;

namespace PolAPI
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
                    StartupHelper.SetupConfig(args, config, hostingContext.HostingEnvironment.EnvironmentName);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseIISIntegration();
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog();
                })
                .ConfigureServices((hosted, services) =>
                {
                    services.AddHostedService<IMailSenderHostedService>(serviceProvider =>
                    {
                        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                        return new MailSenderHostedService(
                            serviceProvider.GetRequiredService<ILogger>(),
                            serviceProvider,
                            serviceProvider.GetRequiredService<IMailService>(),
                            configuration.GetValue<string>("MailFromAddress"),
                            configuration.GetValue<int>("MailSendingIntervalInSeconds")
                        );
                    });
                });
        }
    }
}