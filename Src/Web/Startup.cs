using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using CleanArchitecture.Infrastructure.DomainEvents;
using Domain.Interfaces;
using Domain.SharedKernel;
using Infrastructure.Data;
using Infrastructure.DomainEvents.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Serilog;

namespace Web
{
    public class Startup
    {
        private static readonly string _politikerafregningApi = "Politikerafregning API";
        private static readonly string _version = "v1";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });

            services.AddDbContext<PolDbContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("PolDb");
                options
                    .UseMySql(connectionString, mySqlOptions => mySqlOptions
                        .ServerVersion(new Version(10, 4, 12, 0), ServerType.MySql)
                    );
            });
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IRepository, EfRepository>();

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build())
                .CreateLogger();
            services.AddSingleton<ILogger>(logger);
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc(_version, new OpenApiInfo {Title = _politikerafregningApi, Version = _version});
            });
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<ITravelExpenseValidator, TravelExpenseValidator>();

            // We'll be needing this construct for other thing, keeping it in code for now.
            //Assembly
            //    .GetEntryAssembly()
            //    .GetTypesAssignableFrom<ITravelExpenseValidatorRule>()
            //    .ForEach(t =>
            //    {
            //        services.AddScoped(typeof(ITravelExpenseValidatorRule), t);
            //    });

            services.AddScoped<IHandle<TravelExpenseUpdatedDomainEvent>, TravelExpenseUpdatedNotificationHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger,
            IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                logger.Information("In Development environment");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                // Migrate database as needed.
                var context = serviceScope.ServiceProvider.GetRequiredService<PolDbContext>();
                context.Database.Migrate();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(
                c => c.SwaggerEndpoint("/swagger/v1/swagger.json", _politikerafregningApi + " " + _version));

            app.UseStaticFiles();
            if (!env.IsDevelopment()) app.UseSpaStaticFiles();

            app.UseRouting();

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