using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using SharedWouldBeNugets;
using TestHelpers;

namespace Kata
{
    public class Program
    {
        //private static OidcClient _oidcClient;
        private static int _exitCode;
        private static JwtUser[] _jwtUsers;
        private static Properties _properties;
        private static ILogger _logger;

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

            var startTime = DateTime.Now;

            try
            {
                var serviceCollection = new ServiceCollection()
                    .AddScoped<ILogger>(s => _logger)
                    .AddScoped<IRestClientProvider, RestClientProvider>()
                    .AddScoped(s => _properties)
                    .AddScoped(s => _jwtUsers)
                    .AddScoped<IClientContext>(s => new ClientContext())
                    .AddScoped<IPolicyService,PolicyService>()
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

                    new KataStepDescriptor("ResetTestData").AsUser("alice").WithVerification(c=>c.DatabaseResetResponse!=null),

                    new KataStepDescriptor("SendWaitingSubmissions").AsUser("alice"),

                    //new KataStepDescriptor("GetUserInfo").AsUser("freddie").WithVerification(c=>c.UserInfoGetResponse!=null && c.UserInfoGetResponse.UserCustomerInfo.All(x=>x.UserCustomerStatus!=0)),

                    new KataStepDescriptor("GetUserInfo").AsUser("alice").WithVerification(c=>c.UserInfoGetResponse!=null),
                    new KataStepDescriptor("GetAllTravelExpenses").AsUser("alice").WithVerification(c=>c.TravelExpenseGetResponse?.Result!=null && c.TravelExpenseGetResponse.Result.Count()==TestData.GetNumberOfTestDataTravelExpenses()),
                    new KataStepDescriptor("CreateNewTravelExpense").AsUser("alice").WithVerification(c=>c.TravelExpenseCreateResponse!=null && c.TravelExpenseCreateResponse.Id!=Guid.Empty),
                    new KataStepDescriptor("GetSingleTravelExpense").AsUser("alice").WithVerification(c=>c.TravelExpenseGetByIdResponse!=null && c.TravelExpenseGetByIdResponse.Result.Id==c.TravelExpenseCreateResponse.Id && c.TravelExpenseGetByIdResponse.Result.ArrivalPlace!=null && c.TravelExpenseGetByIdResponse.Result.DeparturePlace!=null),
                    new KataStepDescriptor("UpdateTravelExpense").AsUser("alice").WithVerification(c=>c.TravelExpenseUpdateResponse!=null),
                    new KataStepDescriptor("GetAllTravelExpenses").AsUser("alice").WithVerification(c=>c.TravelExpenseGetResponse?.Result!=null && c.TravelExpenseGetResponse.Result.Count()==TestData.GetNumberOfTestDataTravelExpenses()+1),
                    new KataStepDescriptor("GetFlowSteps").AsUser("alice").WithVerification(c=>c.FlowStepGetResponse .Result.Any()),
                    new KataStepDescriptor("ApproveLatestTravelExpense").AsUser("alice"),

                    new KataStepDescriptor("GetUserInfo").AsUser("bob").WithVerification(c=>c.UserInfoGetResponse!=null),
                    new KataStepDescriptor("GetFlowSteps").AsUser("bob").WithVerification(c=>c.FlowStepGetResponse .Result.Any()),
                    new KataStepDescriptor("GetAllTravelExpenses").AsUser("bob").WithVerification(c=>c.TravelExpenseGetResponse?.Result!=null && c.TravelExpenseGetResponse.Result.Count()==1),
                    new KataStepDescriptor("CertifyLatestTravelExpense").AsUser("bob"),

                    new KataStepDescriptor("GetUserInfo").AsUser("charlie").WithVerification(c=>c.UserInfoGetResponse!=null),
                    new KataStepDescriptor("GetFlowSteps").AsUser("charlie").WithVerification(c=>c.FlowStepGetResponse .Result.Any()),
                    new KataStepDescriptor("GetAllTravelExpenses").AsUser("charlie").WithVerification(c=>c.TravelExpenseGetResponse?.Result!=null && c.TravelExpenseGetResponse.Result.Count()==1),
                    new KataStepDescriptor("AssignForPaymentLatestTravelExpense").AsUser("charlie"),

                    new KataStepDescriptor("GetUserInfo").AsUser("dennis").WithVerification(c=>c.UserInfoGetResponse!=null),
                    new KataStepDescriptor("GetCustomerUsers").AsUser("dennis"),
                    new KataStepDescriptor("ChangeUserCustomerStatus").AsUser("dennis"),

                    new KataStepDescriptor("GetUserInfo").AsUser("edward").WithVerification(c=>c.UserInfoGetResponse!=null && c.UserInfoGetResponse.UserCustomerInfo.All(x=>x.UserCustomerStatus!=0)),

                    new KataStepDescriptor("GetUserInfo").AsUser("dennis").WithVerification(c=>c.UserInfoGetResponse!=null && c.UserInfoGetResponse.UserCustomerInfo.All(x=>x.UserCustomerStatus!=0)),
                    new KataStepDescriptor("SendInvitationEmails").AsUser("dennis").WithVerification(c=>c.CustomerInvitationsPostResponse!=null ),

                    new KataStepDescriptor("GetUserInfo").AsUser("edward").WithVerification(c=>c.UserInfoGetResponse!=null && c.UserInfoGetResponse.UserCustomerInfo.All(x=>x.UserCustomerStatus!=0)),

                    new KataStepDescriptor("AddCustomersToUser").AsUser("freddie").WithVerification(c=>c.UserCustomerPostResponse.Ids.Count().Equals(1)), 

                };

                foreach (var kataStepDescriptor in kataStepDescriptors)
                {
                    var stepStart = DateTime.Now;
                    var step = kataStepProvider.GetStep(kataStepDescriptor.Identifier);

                    try
                    {
                        if(step==null)
                            throw new InvalidOperationException("No kata step matches descriptor: " + kataStepDescriptor.Identifier);

                        _logger.Information("About to execute step: {kataStepIdentifier}, {kataStepType}, executing as {userName}",
                            kataStepDescriptor.Identifier, step.GetType().Name, kataStepDescriptor.NameOfLoggedInUser);

                        await step.ExecuteAndVerifyAsync(
                            kataStepDescriptor.NameOfLoggedInUser,
                            kataStepDescriptor.VerificationFunc);
                        var stepEnd= DateTime.Now;

                        _logger.Information("Done executing step: {kataStepIdentifier}, {kataStepType}, executing as {userName}, {secondsElapsed}",
                            kataStepDescriptor.Identifier, step.GetType().Name, kataStepDescriptor.NameOfLoggedInUser, stepEnd.Subtract(stepStart).TotalSeconds);
                    }
                    catch (Exception)
                    {
                        var logMessage = $"Error executing step: {kataStepDescriptor.Identifier}, {step.GetType().Name}, executing as {kataStepDescriptor.NameOfLoggedInUser}";
                        _logger.Error(logMessage);
                        throw;
                    }

                    Thread.Sleep(opts.SleepMs);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e,"During Kata steps...");

                if (opts.UsePrompt)
                    Console.ReadLine();

                throw;
            }

            var endTime = DateTime.Now;

            _logger.Information("Kata executed without errors after {secondsElapsed}s on {system}!", endTime.Subtract(startTime).TotalSeconds, opts.SutName);

            if (opts.UsePrompt)
            {
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
        }

        private static async Task HandleParseErrorAsync(IEnumerable<Error> errs)
        {
            await Task.CompletedTask;

            _exitCode = 1;
        }

        public static async Task<int> Main(string[] args)
        {
            try
            {
                var cb = new ConfigurationBuilder();
                StartupHelper.SetupConfig(new string[] { }, cb, "Development");
                _logger = StartupHelper.CreateLogger(cb.Build(), "Kata");

                var parserResult = Parser.Default.ParseArguments<Options>(args);
                var withParsedAsync = await parserResult
                    .WithParsedAsync(RunOptions);
                await withParsedAsync.WithNotParsedAsync(HandleParseErrorAsync);
            }
            catch (Exception)
            {
                _logger.Information("Kata executed with errors !");
                _exitCode = 1;
            }

            return await Task.FromResult(_exitCode);
        }
    }
}