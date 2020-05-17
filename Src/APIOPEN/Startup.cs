using API.Shared;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SharedWouldBeNugets;

namespace APIOPEN
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private const string Title = CommonApi.Title + "(OPEN) ";

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPolApi(_configuration, false, Title, "APIOPEN");
            services.AddPolDatabase(_configuration, _environment.EnvironmentName);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger,
            PolDbContext polDbContext, IDbSeeder dbSeeder, IPolicyService policyService)
        {
            logger.Information("Starting Politikerafregning APIOPEN...");

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            policyService.DatabaseMigrationAndSeedingPolicy.Execute(() =>
            {
                logger.Information("Starting Db Migration and Seeding...");
                polDbContext.Database.Migrate();
                dbSeeder.RemoveTestData().Wait();
                dbSeeder.Seed();
                logger.Information("Done Db Migration and Seeding...");
            });

            app.UseCors(options =>
            {
                options
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(ImproventoGlobals.AllowedCorsOrigins)
                    .AllowCredentials();
            });

            app.UseHttpsRedirection();
            app.UseMiddleware<ErrorWrappingMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(
                c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", Title + CommonApi.Version);
                    c.DocumentTitle = "API OPEN";
                });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            logger.Information("TravelExpense APIOPEN started. Version=" + _configuration.GetValue<string>("Version"));
        }
    }
}