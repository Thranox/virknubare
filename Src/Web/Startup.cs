using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using API.Shared;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Interfaces;
using Domain.SharedKernel;
using IDP.Services;
using Infrastructure.Data;
using Infrastructure.DomainEvents;
using Infrastructure.DomainEvents.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using Web.ActionFilters;
using Web.MapperProfiles;

namespace Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        private readonly IConfiguration _configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPolApi(_configuration, true);

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
            IConfiguration configuration)
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            }
            app.UseHsts();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                // Migrate database as needed.
                var context = serviceScope.ServiceProvider.GetRequiredService<PolDbContext>();
                //context.Database.ExecuteSqlRaw("drop table __efmigrationshistory; \r\ndrop table flowstepuserpermissionentity; \r\ndrop table flowstepentity; \r\ndrop table travelexpenses; \r\ndrop table userentity; \r\ndrop table customerentities; ");
                context.Database.Migrate();
                if (!env.IsProduction())
                {
                    context.Seed();
                }
            }

            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(
                c => c.SwaggerEndpoint("/swagger/v1/swagger.json", CommonApi.Title + " " +CommonApi.Version));

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