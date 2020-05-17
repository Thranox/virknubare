using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using CommandLine;
using Domain;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Serilog.Core;
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
                // Wait for api being up
                var kataApiRetryPolicy = new PolicyService(logger).KataApiRetryPolicy;
                await kataApiRetryPolicy.Execute(async () =>
                 {
                     try
                     {
                         logger.Information("Trying to reach swagger page...");
                         var cancellationTokenSource = new CancellationTokenSource(100);
                         var httpClient = new HttpClient();
                         await httpClient.GetAsync(new Uri (_properties.ApiEndpoint + "/swagger/index.html"), cancellationTokenSource.Token);
                         logger.Information("Done trying to reach swagger page...");
                     }
                     catch (TaskCanceledException e)
                     {
                         logger.Debug("Timeout");
                     }
                 });

                Thread.Sleep(opts.SleepMs);
                await ResetTestDataInDatabase(logger);
                Thread.Sleep(opts.SleepMs);
                var userInfoGetResponse = await GetUserInfo(logger);
                Thread.Sleep(opts.SleepMs);
                await GetAllTravelExpenses(logger);
                Thread.Sleep(opts.SleepMs);
                await CreateNewTravelExpense(logger, userInfoGetResponse);
                Thread.Sleep(opts.SleepMs);
                logger.Information("Done");
            }
            catch (Exception e)
            {
                logger.Error(e, "During Kata steps...");
                throw;
            }
            logger.Information("Kata executed without errors!");

            if (opts.UsePrompt)
            {
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
        }

        private static async Task CreateNewTravelExpense(Logger logger, UserInfoGetResponse userInfoGetResponse)
        {
            // Still alice, create new Travel Expense
            logger.Debug("Creating TravelExpense...");
            var restClient = GetRestClient("alice");
            var restRequest = new RestRequest(
                    new Uri("/travelexpenses", UriKind.Relative)
                )
                .AddJsonBody(new TravelExpenseCreateDto
                {
                    Description = "From kata",
                    CustomerId = userInfoGetResponse.UserCustomerInfo
                        .First(x => x.UserCustomerStatus != (int)UserStatus.Initial).CustomerId
                });
            var o = await restClient.PostAsync<TravelExpenseGetResponse>(restRequest);
            logger.Debug("Created TravelExpense {object}", JsonConvert.SerializeObject(o));
        }

        private static async Task GetAllTravelExpenses(Logger logger)
        {
            // As Alice (politician), get all Travel Expenses (that is, all she can see)
            logger.Debug("Getting TravelExpenses...");
            var restClient = GetRestClient("alice");
            var travelExpenseGetResponse =
                await restClient.GetAsync<TravelExpenseGetResponse>(
                    new RestRequest(new Uri("/travelexpenses", UriKind.Relative)));
            logger.Debug("travelExpenseGetResponse - {travelExpenseGetResponse}",
                JsonConvert.SerializeObject(travelExpenseGetResponse));
        }

        private static async Task<UserInfoGetResponse> GetUserInfo(Logger logger)
        {
            // As Alice, get customers from the UserInfo endpoint
            logger.Debug("Getting UserInfoGetResponse...");
            var restClient = GetRestClient("alice");
            var result =
                await restClient.ExecuteAsync<UserInfoGetResponse>(
                    new RestRequest(new Uri("/userinfo", UriKind.Relative), Method.GET));
            var userInfoGetResponse = JsonConvert.DeserializeObject<UserInfoGetResponse>(result.Content);
            logger.Debug("userInfoGetResponse - {userInfoGetResponse}",
                JsonConvert.SerializeObject(userInfoGetResponse));
            return userInfoGetResponse;
        }

        private static async Task ResetTestDataInDatabase(Logger logger)
        {
            // Reset test data in database. This requires God access.
            logger.Debug("Resetting Database...");
            var restClient = GetRestClient("alice");
            await restClient.PostAsync<object>(new RestRequest(new Uri("/Admin/DatabaseReset", UriKind.Relative)));
            logger.Debug("Database reset.");
        }

        private static IRestClient GetRestClient(string jwtUserName)
        {
            IRestClient restClient = new RestClient(new Uri(_properties.ApiEndpoint));
            restClient.AddDefaultHeader("Authorization",
                $"Bearer {_jwtUsers.Single(x => x.Name == jwtUserName).AccessToken}");
            return restClient;
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