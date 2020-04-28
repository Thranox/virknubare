using API.Shared;
using IDP.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

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
            services.AddPolApi(_configuration, true, Title);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });

            services.AddDbContext<PolDbContext>(options =>
            {
                var connectionStringService = new ConnectionStringService(_configuration, _environment.EnvironmentName);
                var connectionString = connectionStringService.GetConnectionString("PolConnection");
                options.UseSqlServer(connectionString);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger,
            IConfiguration configuration, IDbSeeder dbSeeder)
        {
            logger.Information("------------------------------------------------------------");
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

            dbSeeder.Seed();

            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(
                c => c.SwaggerEndpoint("/swagger/v1/swagger.json", Title + " " + CommonApi.Version));

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
            logger.Information("------------------------------------------------------------");
        }
    }
}