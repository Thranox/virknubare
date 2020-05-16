using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using CommandLine;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using SharedWouldBeNugets;

namespace Kata
{
    public class Program
    {
        private static readonly string _authority = "https://localhost:5001";
        private static readonly string _api = "https://localhost:44348";

        private static OidcClient _oidcClient;
        private static readonly HttpClient _apiClient = new HttpClient {BaseAddress = new Uri(_api)};
        private static int _exitCode;
        private static JwtUser[] _jwtUsers;

        private static async Task RunOptions(Options opts)
        {
            if (!Directory.Exists(opts.JwtDir))
                throw new ArgumentException("JwtDir not found: " + opts.JwtDir);

            var expectedDir = Path.Combine(opts.JwtDir, opts.SutName);
            if (!Directory.Exists(expectedDir))
                throw new ArgumentException("Sutname dir not found under JwtDir : " + expectedDir);

            var json = File.ReadAllText(Path.Combine(expectedDir, "jwts.json"));
            _jwtUsers = JsonConvert.DeserializeObject<JwtUser[]>(json);

            var cb=new ConfigurationBuilder();
            StartupHelper.SetupConfig(new string[]{}, cb, "Development");

            var logger = StartupHelper.CreateLogger(cb.Build(), "Kata");

            Console.WriteLine("Press enter...");
            Console.ReadLine();

            do
            {
                HttpResponseMessage response;

                // Reset test data in database. This requires God access.
                logger.Debug("Resetting Database...");
                var restClient = GetRestClient("alice");
                await restClient.PostAsync<object>(new RestRequest(new Uri("/Admin/DatabaseReset", UriKind.Relative)));
                logger.Debug("Database reset.");

                Console.WriteLine("Press enter...");
                Console.ReadLine();

                // As Alice, get customers from the UserInfo endpoint
                logger.Debug("Getting UserInfoGetResponse...");
                restClient = GetRestClient("alice");
                var result = await restClient.ExecuteAsync<UserInfoGetResponse>(new RestRequest(new Uri("/userinfo", UriKind.Relative),Method.GET));
                var userInfoGetResponse = JsonConvert.DeserializeObject<UserInfoGetResponse>(result.Content);
                logger.Debug("userInfoGetResponse - {userInfoGetResponse}",JsonConvert.SerializeObject( userInfoGetResponse));

                Console.WriteLine("Press enter...");
                Console.ReadLine();

                // As Alice (politician), get all Travel Expenses (that is, all she can see)
                logger.Debug("Getting TravelExpenses...");
                restClient = GetRestClient("alice");
                var travelExpenseGetResponse = await restClient.GetAsync<TravelExpenseGetResponse>(new RestRequest(new Uri("/travelexpenses",UriKind.Relative)));
                logger.Debug("travelExpenseGetResponse - {travelExpenseGetResponse}", JsonConvert.SerializeObject( travelExpenseGetResponse));

                Console.WriteLine("Press enter...");
                Console.ReadLine();

                // Still alice, create new Travel Expense
                logger.Debug("Creating TravelExpense...");
                restClient = GetRestClient("alice");
                var travelExpenseCreateDto = new TravelExpenseCreateDto(){Description = "From kata",CustomerId = userInfoGetResponse.UserCustomerInfo.First(x=>x.UserCustomerStatus=="Registered").CustomerId};
                var restRequest = new RestRequest(new Uri("/travelexpenses", UriKind.Relative))
                    .AddJsonBody(travelExpenseCreateDto);
                var o = await restClient.PostAsync<TravelExpenseGetResponse>(restRequest);
                logger.Debug("Created TravelExpense {object}",JsonConvert.SerializeObject( o));

                Console.WriteLine("Press enter...");
                Console.ReadLine();
            } while (true);
        }

        private static IRestClient GetRestClient(string jwtUserName)
        {
            IRestClient restClient = new RestClient(new Uri(_api));
            restClient.AddDefaultHeader("Authorization", $"Bearer {_jwtUsers.Single(x => x.Name == jwtUserName).AccessToken}");
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

        //public static async Task RunAsync()
        //{
        //    await Login();
        //}

        //private static async Task Login()
        //{
        //    var browser = new SystemBrowser(ImproventoGlobals.LocalKataPort);
        //    var redirectUri = ImproventoGlobals.LocalKataRedirect;

        //    var options = new OidcClientOptions
        //    {
        //        Authority = _authority,
        //        ClientId = ImproventoGlobals.AngularClientId,
        //        RedirectUri = redirectUri,
        //        Scope = "openid profile teapi",
        //        FilterClaims = false,
        //        Browser = browser,
        //        Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
        //        ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect,
        //        LoadProfile = true,
        //    };
        //    options.Policy.Discovery.ValidateIssuerName = false;
        //    options.PostLogoutRedirectUri = redirectUri;

        //    ConfigurationBuilder cb=new ConfigurationBuilder();
        //    var serilog = StartupHelper.CreateLogger(cb.Build(), "Kata");

        //    //var serilog = new LoggerConfiguration()
        //    //    .MinimumLevel.Verbose()
        //    //    .Enrich.FromLogContext()
        //    //    .WriteTo.LiterateConsole(
        //    //        outputTemplate:
        //    //        "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message}{NewLine}{Exception}{NewLine}")
        //    //    .CreateLogger();

        //    options.LoggerFactory.AddSerilog(serilog);

        //    _oidcClient = new OidcClient(options);
        //    LoginResult result = null;
        //    var success = false;
        //    do
        //    {
        //        try
        //        {
        //            result = await _oidcClient.LoginAsync(new LoginRequest());
        //            await _oidcClient.LogoutAsync(new LogoutRequest(){IdTokenHint = result.IdentityToken});

        //            success = true;
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine("Failed: " + e.Message);
        //            Thread.Sleep(1000);
        //        }
        //    } while (!success);

        //    var tempPath = Path.GetTempPath();
        //    File.WriteAllText(Path.Combine(tempPath, "credentials.pol"), JsonConvert.SerializeObject(result.AccessToken));

        //    ShowResult(result);
        //    await NextSteps(result);
        //}

        private static void ShowResult(LoginResult result)
        {
            if (result.IsError)
            {
                Console.WriteLine("\n\nError:\n{0}", result.Error);
                return;
            }

            Console.WriteLine("\n\nClaims:");
            foreach (var claim in result.User.Claims) Console.WriteLine("{0}: {1}", claim.Type, claim.Value);

            Console.WriteLine($"\nidentity token: {result.IdentityToken}");
            Console.WriteLine($"access token:   {result.AccessToken}");
            Console.WriteLine($"refresh token:  {result?.RefreshToken ?? "none"}");
        }

        //private static async Task NextSteps(LoginResult result)
        //{
        //    var currentAccessToken = result.AccessToken;
        //    var currentRefreshToken = result.RefreshToken;

        //    var menu =
        //        " x:exit \n b:call api All \n c:call api with route \n d:post api with body \n e:call api with query parameter";
        //    if (currentRefreshToken != null) menu += "r:refresh token";

        //    await CallApi(currentAccessToken);

        //    while (true)
        //    {
        //        Console.Write(menu);
        //        Console.Write("\n");
        //        var key = Console.ReadKey();

        //        if (key.Key == ConsoleKey.X) return;
        //        if (key.Key == ConsoleKey.B) await CallApi(currentAccessToken);
        //        if (key.Key == ConsoleKey.C) await CallApiwithRouteValue(currentAccessToken, "phil");
        //        if (key.Key == ConsoleKey.D) await CallApiwithBodyValue(currentAccessToken, "mike");
        //        if (key.Key == ConsoleKey.E) await CallApiwithQueryStringParam(currentAccessToken, "orange");
        //        if (key.Key == ConsoleKey.R)
        //        {
        //            var refreshResult = await _oidcClient.RefreshTokenAsync(currentRefreshToken);
        //            if (result.IsError)
        //            {
        //                Console.WriteLine($"Error: {refreshResult.Error}");
        //            }
        //            else
        //            {
        //                currentRefreshToken = refreshResult.RefreshToken;
        //                currentAccessToken = refreshResult.AccessToken;

        //                Console.WriteLine($"access token:   {result.AccessToken}");
        //                Console.WriteLine($"refresh token:  {result?.RefreshToken ?? "none"}");
        //            }
        //        }
        //    }
        //}

        //private static async Task CallApi(string currentAccessToken)
        //{
        //    _apiClient.SetBearerToken(currentAccessToken);
        //    var response = await _apiClient.GetAsync("/WeatherForecast");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var json = JArray.Parse(await response.Content.ReadAsStringAsync());
        //        Console.WriteLine("\n" + json);
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Error: {response.ReasonPhrase}");
        //    }
        //}

        //private static async Task CallApiwithBodyValue(string currentAccessToken, string user)
        //{
        //    _apiClient.SetBearerToken(currentAccessToken);
        //    var response = await _apiClient.PostAsJsonAsync(
        //        "/api/values",
        //        new BodyData {User = user}
        //    );

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var result = await response.Content.ReadAsStringAsync();
        //        Console.WriteLine($"\n{result}");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Error: {response.ReasonPhrase}");
        //    }
        //}

        //private static async Task CallApiwithRouteValue(string currentAccessToken, string user)
        //{
        //    _apiClient.SetBearerToken(currentAccessToken);
        //    var response = await _apiClient.GetAsync($"/api/values/{user}");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var result = await response.Content.ReadAsStringAsync();
        //        Console.WriteLine($"\n{result}");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Error: {response.ReasonPhrase}");
        //    }
        //}

        //private static async Task CallApiwithQueryStringParam(string currentAccessToken, string fruit)
        //{
        //    _apiClient.SetBearerToken(currentAccessToken);
        //    var response = await _apiClient.GetAsync($"/api/values/q?fruit={fruit}");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var result = await response.Content.ReadAsStringAsync();
        //        Console.WriteLine($"\n{result}");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Error: {response.ReasonPhrase}");
        //    }
        //}
    }

    //public class BodyData
    //{
    //    public string User { get; set; }
    //}
}