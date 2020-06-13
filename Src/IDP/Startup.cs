using IdentityServer4.Services;
using IdentityServerAspNetIdentit.Data;
using IdentityServerAspNetIdentit.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SharedWouldBeNugets;
using TestHelpers;

namespace IDP
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (Configuration.GetValue<bool>("UseRealEmailSender"))
            {
                services.AddScoped<IMailService, MailService>();
            }
            else
            {
                services.AddScoped<IMailService, FakeMailService>();
            }

            services.AddScoped<IEmailFactory, EmailFactory>();
            services.AddScoped<ILogger>(s=>Log.Logger);
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddControllersWithViews();

            // configures IIS out-of-proc settings (see https://github.com/aspnet/AspNetCore/issues/14882)
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            // configures IIS in-proc settings
            services.Configure<IISServerOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionStringService = new ConnectionStringService(Configuration,Environment.EnvironmentName);

                var connectionString =connectionStringService.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer(options =>
                {
                    options.IssuerUri = ImproventoGlobals.IssUri;
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.UserInteraction.LoginUrl = "/Account/Login";
                    options.UserInteraction.LogoutUrl = "/Account/Logout";
                })
                .AddInMemoryIdentityResources(Config.Ids)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<ApplicationUser>()
                .AddDeveloperSigningCredential(false, Configuration.GetValue<string>("SigningKey"));// @"c:\keys\signing.rsa");

            services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddGoogle(options =>
                {
                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to http://localhost:5000/signin-google
                    options.ClientId = "copy client ID from Google here";
                    options.ClientSecret = "copy client secret from Google here";
                });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    corsPolicyBuilder => corsPolicyBuilder
                        .WithOrigins("http://localhost:5000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            services.AddSingleton<ICorsPolicyService, DefaultCorsPolicyService>();
        }

        public void Configure(IApplicationBuilder app, ILogger logger, ICorsPolicyService corsPolicyService)
        {
            if (corsPolicyService is DefaultCorsPolicyService defaultCorsPolicyService)
            {
                defaultCorsPolicyService.AllowAll = true;
            }

            Log.Information("Ensuring database is migrated and seeded...");
            var connectionStringService = new ConnectionStringService(Configuration, Environment.EnvironmentName);
            var connectionString = connectionStringService.GetConnectionString("DefaultConnection");
            SeedData.EnsureSeedData(connectionString);
            Log.Information("Done ensuring database is migrated and seeded.");

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
            
            app.UseIdentityServer();
            app.UseCors("CorsPolicy");
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}