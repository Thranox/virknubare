using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using SharedWouldBeNugets;

namespace Kata
{
    public class Program
    {
        //private static OidcClient _oidcClient;
        private static int _exitCode;
        private static JwtUser[] _jwtUsers;
        private static Properties _properties;

        private static async Task RunOptions(Options opts)
        {
            if (!Directory.Exists(opts.JwtDir))
                throw new ArgumentException("JwtDir not found: " + opts.JwtDir);

            var expectedDir = Path.Combine(opts.JwtDir, opts.SutName);
            if (!Directory.Exists(expectedDir))
                throw new ArgumentException("Sutname dir not found under JwtDir : " + expectedDir);

            var json = File.ReadAllText(Path.Combine(expectedDir, "jwts.json"));
            _jwtUsers = JsonConvert.DeserializeObject<JwtUser[]>(json);

            var propertiesJson = File.ReadAllText(Path.Combine(expectedDir, "properties.json"));
            _properties = JsonConvert.DeserializeObject<Properties>(propertiesJson);

            var cb = new ConfigurationBuilder();
            StartupHelper.SetupConfig(new string[] { }, cb, "Development");
            var logger = StartupHelper.CreateLogger(cb.Build(), "Kata");

            try
            {
                var serviceCollection = new ServiceCollection()
                    .AddScoped<ILogger>(s => logger)
                    .AddScoped<IRestClientProvider, RestClientProvider>()
                    .AddScoped(s => _properties)
                    .AddScoped(s => _jwtUsers)
                    .AddScoped<IClientContext>(s=>new ClientContext())
                    .AddScoped<IKataStepProvider, KataStepProvider>();
                Assembly
                    .GetAssembly(typeof(IKataStep))
                    .GetTypesAssignableFrom<IKataStep>()
                    .ForEach(t => { serviceCollection.AddScoped(typeof(IKataStep), t); });

                var serviceProvider = serviceCollection
                    .BuildServiceProvider();

                var kataStepProvider = serviceProvider.GetRequiredService<IKataStepProvider>();
                var kataStepDescriptors = new[]
                {
                    new KataStepDescriptor("VerifySwaggerUp"),
                    new KataStepDescriptor("ResetTestData"),
                    new KataStepDescriptor("GetUserInfo")
                };

                foreach (var kataStepDescriptor in kataStepDescriptors)
                {
                    var step = kataStepProvider.GetStep(kataStepDescriptor.Identifier);
                    await step.ExecuteAsync(_properties);
                    Thread.Sleep(opts.SleepMs);
                }

                //Thread.Sleep(opts.SleepMs);
                //var userInfoGetResponse = await GetUserInfo(logger);
                //Thread.Sleep(opts.SleepMs);
                //await GetAllTravelExpenses(logger);
                //Thread.Sleep(opts.SleepMs);
                //await CreateNewTravelExpense(logger, userInfoGetResponse);
                //Thread.Sleep(opts.SleepMs);
                //logger.Information("Done");
            }
            catch (Exception e)
            {
                logger.Error(e, "During Kata steps...");
                throw;
            }

            logger.Information("Kata executed without errors on {system}!", opts.SutName);

            if (opts.UsePrompt)
            {
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
        }


        private static async Task HandleParseError(IEnumerable<Error> errs)
        {
            _exitCode = 1;
        }

        public static async Task<int> Main(string[] args)
        {
            try
            {
                var parserResult = Parser.Default.ParseArguments<Options>(args);
                var withParsedAsync = await parserResult
                    .WithParsedAsync(RunOptions);
                await withParsedAsync.WithNotParsedAsync(HandleParseError);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _exitCode = 1;
            }

            return await Task.FromResult(_exitCode);
        }
    }
}