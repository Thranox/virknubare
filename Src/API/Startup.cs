using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Shared;
using API.Shared.Controllers;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IDP.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        private IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILogger>(Log.Logger);

            var assembly = typeof(TravelExpenseController).Assembly;
            services.AddControllersWithViews()
                .AddApplicationPart(assembly);

            services.AddDbContext<PolDbContext>(options =>
            {
                var connectionStringService = new ConnectionStringService(_configuration, _environment.EnvironmentName);
                var connectionString = connectionStringService.GetConnectionString("PolConnection");
                options.UseSqlServer(connectionString);
            });

            services.AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
            //services.AddTransient<IProfileService, MyProfileService>();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger)
        {
            logger.Information("------------------------------------------------------------");
            logger.Information("Starting Politikerafregning API...");

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthentication();

            app.UseHttpsRedirection();
            
            app.UseSwagger();
            app.UseSwaggerUI(
                c => c.SwaggerEndpoint("/swagger/v1/swagger.json",CommonApi. Title + " " +CommonApi. Version));

            app.UseMvc();
            logger.Information("TravelExpense API started. Version=" + _configuration.GetValue<string>("Version"));
            logger.Information("------------------------------------------------------------");
        }
    }
}