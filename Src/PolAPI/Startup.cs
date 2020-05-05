using System;
using API.Shared;
using IDP.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger,
            PolDbContext polDbContext, IDbSeeder dbSeeder, IPolicyService policyService)
        {
            logger.Information("Starting Politikerafregning API...");

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            policyService.DatabaseMigrationAndSeedingPolicy.Execute(() =>
            {
                logger.Information("Starting Db Migration and Seeding...");
                polDbContext.Database.Migrate();
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
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(
                c => c.SwaggerEndpoint("v1/swagger.json", Title + " " + CommonApi.Version));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            logger.Information("TravelExpense API started. Version=" + _configuration.GetValue<string>("Version"));
        }
    }
}