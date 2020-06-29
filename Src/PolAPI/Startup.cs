using System;
using System.Linq;
using API.Shared;
using API.Shared.ActionFilters;
using API.Shared.Controllers;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SharedWouldBeNugets;

namespace PolAPI
{
    public class Startup
    {
        private const string Title = CommonApi.Title;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPolApi(_configuration, true, Title, "PolAPI");
            services.AddPolDatabase(_configuration, _environment.EnvironmentName);

            services.AddControllersWithViews(options =>
                {
                    // Include handling of Domain Exceptions
                    options.Filters.Add<HttpResponseExceptionFilter>();
                    // Log all entries and exits of controller methods.
                    options.Filters.Add<MethodLoggingActionFilter>();

                    // If desired, be set up a global Authorize filter
                    if (true)
                    {
                        var policyRequiringAuthenticatedUser = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
                        options.Filters.Add(new AuthorizeFilter(policyRequiringAuthenticatedUser));
                    }
                })
                .AddApplicationPart(typeof(TravelExpenseController).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger, IDbSeeder dbSeeder, IPolicyService policyService)
        {
            logger.Information("Starting Politikerafregning API...");

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            policyService.DatabaseMigrationAndSeedingPolicy.ExecuteAsync(async () =>
            {
                logger.Information("Starting Db Migration and Seeding...");
                await dbSeeder.MigrateAsync();

                if (_configuration.GetValue<bool>("RemoveAndReseedOnStartup"))
                {
                    try
                    {
                        logger.Information("Removing test data...");
                        await dbSeeder.RemoveTestDataAsync();
                    }
                    catch (Exception e)
                    {
                        logger.Error(e, "During RemoveTestDataAsync");
                        throw;
                    }
                }

                await dbSeeder.SeedAsync();
                logger.Information("Done Db Migration and Seeding...");
            }).Wait();

            app.UseCors(options =>
            {
                options
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(ImproventoGlobals.AllowedCorsOrigins)
                    .AllowCredentials();
            });
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMiddleware<ErrorWrappingMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(
                c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", Title + CommonApi.Version);
                    c.DocumentTitle = "PolAPI";
                });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            logger.Information("TravelExpense API started. Version=" + _configuration.GetValue<string>("Version")+"\n"+string.Join("\n", _configuration.AsEnumerable(true).Select(x=>x.Key +"="+x.Value)));
        }
    }
}