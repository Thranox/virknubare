using API.Shared;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SharedWouldBeNugets;

namespace Web
{
    public class Startup
    {
        private const string Title = CommonApi.Title + "(Through WEB)";
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPolApi(_configuration, false, Title, "Web");
            services.AddPolDatabase(_configuration, _environment.EnvironmentName);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger,
            IConfiguration configuration, IDbSeeder dbSeeder, PolDbContext polDbContext, IPolicyService policyService)
        {
            logger.Information("Starting Politikerafregning Web...");
            if (env.IsDevelopment())
            {
                logger.Information("In Development environment");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseHsts();

            policyService.DatabaseMigrationAndSeedingPolicy.ExecuteAsync(async() =>
            {
                logger.Information("Starting Db Migration and Seeding...");
                await dbSeeder.MigrateAsync();
                await dbSeeder.SeedAsync();
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

            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(
                c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", Title + " " + CommonApi.Version);
                    c.DocumentTitle = "Web hosted API";
                });

            app.UseStaticFiles();
            if (!env.IsDevelopment()) app.UseSpaStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment()) spa.UseAngularCliServer("start");
            });

            logger.Information("TravelExpense Web started. Version=" + configuration.GetValue<string>("Version"));
        }
    }
}