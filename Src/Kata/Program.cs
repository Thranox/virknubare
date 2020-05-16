using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using Microsoft.Extensions.Configuration;
using NativeConsolePKCEClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using SharedWouldBeNugets;

namespace ConsolePKCEClient
{
    public class Program
    {
        private static readonly string _authority = "https://localhost:5001";
        private static readonly string _api = "https://localhost:44348";

        private static OidcClient _oidcClient;
        private static readonly HttpClient _apiClient = new HttpClient {BaseAddress = new Uri(_api)};

        public static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        public static async Task RunAsync()
        {
            await Login();
        }

        private static async Task Login()
        {
            var browser = new SystemBrowser(ImproventoGlobals.LocalKataPort);
            var redirectUri = ImproventoGlobals.LocalKataRedirect;

            var options = new OidcClientOptions
            {
                Authority = _authority,
                ClientId = ImproventoGlobals.AngularClientId,
                RedirectUri = redirectUri,
                Scope = "openid profile teapi",
                FilterClaims = false,
                Browser = browser,
                Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect,
                LoadProfile = true,
            };
            options.Policy.Discovery.ValidateIssuerName = false;

            ConfigurationBuilder cb=new ConfigurationBuilder();
            var serilog = StartupHelper.CreateLogger(cb.Build(), "Kata");

            //var serilog = new LoggerConfiguration()
            //    .MinimumLevel.Verbose()
            //    .Enrich.FromLogContext()
            //    .WriteTo.LiterateConsole(
            //        outputTemplate:
            //        "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message}{NewLine}{Exception}{NewLine}")
            //    .CreateLogger();

            options.LoggerFactory.AddSerilog(serilog);

            _oidcClient = new OidcClient(options);
            LoginResult result = null;
            var success = false;
            do
            {
                try
                {
                    result = await _oidcClient.LoginAsync(new LoginRequest());
                    success = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed: " + e.Message);
                    Thread.Sleep(1000);
                }
            } while (!success);

            var tempPath = Path.GetTempPath();
            File.WriteAllText(Path.Combine(tempPath, "credentials.pol"), JsonConvert.SerializeObject(result.AccessToken));

            ShowResult(result);
            await NextSteps(result);
        }

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

        private static async Task NextSteps(LoginResult result)
        {
            var currentAccessToken = result.AccessToken;
            var currentRefreshToken = result.RefreshToken;

            var menu =
                " x:exit \n b:call api All \n c:call api with route \n d:post api with body \n e:call api with query parameter";
            if (currentRefreshToken != null) menu += "r:refresh token";

            await CallApi(currentAccessToken);

            while (true)
            {
                Console.Write(menu);
                Console.Write("\n");
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.X) return;
                if (key.Key == ConsoleKey.B) await CallApi(currentAccessToken);
                if (key.Key == ConsoleKey.C) await CallApiwithRouteValue(currentAccessToken, "phil");
                if (key.Key == ConsoleKey.D) await CallApiwithBodyValue(currentAccessToken, "mike");
                if (key.Key == ConsoleKey.E) await CallApiwithQueryStringParam(currentAccessToken, "orange");
                if (key.Key == ConsoleKey.R)
                {
                    var refreshResult = await _oidcClient.RefreshTokenAsync(currentRefreshToken);
                    if (result.IsError)
                    {
                        Console.WriteLine($"Error: {refreshResult.Error}");
                    }
                    else
                    {
                        currentRefreshToken = refreshResult.RefreshToken;
                        currentAccessToken = refreshResult.AccessToken;

                        Console.WriteLine($"access token:   {result.AccessToken}");
                        Console.WriteLine($"refresh token:  {result?.RefreshToken ?? "none"}");
                    }
                }
            }
        }

        private static async Task CallApi(string currentAccessToken)
        {
            _apiClient.SetBearerToken(currentAccessToken);
            var response = await _apiClient.GetAsync("/WeatherForecast");

            if (response.IsSuccessStatusCode)
            {
                var json = JArray.Parse(await response.Content.ReadAsStringAsync());
                Console.WriteLine("\n" + json);
            }
            else
            {
                Console.WriteLine($"Error: {response.ReasonPhrase}");
            }
        }

        private static async Task CallApiwithBodyValue(string currentAccessToken, string user)
        {
            _apiClient.SetBearerToken(currentAccessToken);
            var response = await _apiClient.PostAsJsonAsync(
                "/api/values",
                new BodyData {User = user}
            );

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n{result}");
            }
            else
            {
                Console.WriteLine($"Error: {response.ReasonPhrase}");
            }
        }

        private static async Task CallApiwithRouteValue(string currentAccessToken, string user)
        {
            _apiClient.SetBearerToken(currentAccessToken);
            var response = await _apiClient.GetAsync($"/api/values/{user}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n{result}");
            }
            else
            {
                Console.WriteLine($"Error: {response.ReasonPhrase}");
            }
        }

        private static async Task CallApiwithQueryStringParam(string currentAccessToken, string fruit)
        {
            _apiClient.SetBearerToken(currentAccessToken);
            var response = await _apiClient.GetAsync($"/api/values/q?fruit={fruit}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n{result}");
            }
            else
            {
                Console.WriteLine($"Error: {response.ReasonPhrase}");
            }
        }
    }

    public class BodyData
    {
        public string User { get; set; }
    }
}