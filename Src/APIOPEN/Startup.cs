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

namespace APIOPEN
{
    public class Startup
    {
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
            services.AddPolApi(_configuration, false);
            services.AddControllers();

            services.AddDbContext<PolDbContext>(options =>
            {
                var connectionStringService = new ConnectionStringService(_configuration, _environment.EnvironmentName);
                var connectionString = connectionStringService.GetConnectionString("PolConnection");
                options.UseSqlServer(connectionString);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger,
            PolDbContext polDbContext, IDbSeeder dbSeeder)
        {
            logger.Information("------------------------------------------------------------");
            logger.Information("Starting Politikerafregning APIOPEN...");

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            dbSeeder.Seed();

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(
                c => c.SwaggerEndpoint("v1/swagger.json", CommonApi.Title + " " + CommonApi.Version));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            logger.Information("TravelExpense APIOPEN started. Version=" + _configuration.GetValue<string>("Version"));
            logger.Information("------------------------------------------------------------");
        }
    }
}