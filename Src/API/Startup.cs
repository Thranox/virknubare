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
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
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
            services.AddControllers();
            //services.AddPolApi(_configuration, false);

            //services.AddDbContext<PolDbContext>(options =>
            //{
            //    var connectionStringService = new ConnectionStringService(_configuration, _environment.EnvironmentName);
            //    var connectionString = connectionStringService.GetConnectionString("PolConnection");
            //    options.UseSqlServer(connectionString);
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger)
        {
            logger.Information("------------------------------------------------------------");
            logger.Information("Starting Politikerafregning API...");

            //if (env.IsDevelopment())
            //{
            //    logger.Information("In Development environment");
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //}

            //app.UseHsts();

            //using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            //{
            //    // Migrate database as needed.
            //    var context = serviceScope.ServiceProvider.GetRequiredService<PolDbContext>();
            //    //context.Database.ExecuteSqlRaw("drop table __efmigrationshistory; \r\ndrop table flowstepuserpermissionentity; \r\ndrop table flowstepentity; \r\ndrop table travelexpenses; \r\ndrop table userentity; \r\ndrop table customerentities; ");
            //    context.Database.Migrate();
            //    if (!env.IsProduction()) context.Seed();
            //}

            //app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            //app.UseAuthentication();
            //app.UseHttpsRedirection();

            //app.UseSwagger();
            //app.UseSwaggerUI(
            //    c => c.SwaggerEndpoint("/swagger/v1/swagger.json", CommonApi.Title + " " + CommonApi.Version));

            //app.UseStaticFiles();

            //app.UseRouting();
            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        "default",
            //        "{controller}/{action=Index}/{id?}");
            //});


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });




            logger.Information("TravelExpense API started. Version=" + _configuration.GetValue<string>("Version"));
            logger.Information("------------------------------------------------------------");
        }
    }
}