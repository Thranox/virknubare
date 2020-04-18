using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using CleanArchitecture.Infrastructure.DomainEvents;
using Domain.Interfaces;
using Domain.SharedKernel;
using IdentityServer4.AccessTokenValidation;
using Infrastructure.Data;
using Infrastructure.DomainEvents.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Serilog;
using Web.ActionFilters;
using Web.Controllers;
using Web.Services;

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
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
            services.AddSingleton<ILogger>(logger);

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:44305";
                    options.ApiName = "teapi";
                    options.RequireHttpsMetadata = false;
                });

            services.AddControllersWithViews(
                options =>
                {
                    // Include handling of Domain Exceptions
                    options.Filters.Add(new HttpResponseExceptionFilter(logger));
                    // Log all entries and exits of controller methods.
                    options.Filters.Add(new MethodLoggingActionFilter(logger));
                    // Find user for request

                    if (Configuration.GetValue<bool>("UseAuthentication"))
                    {
                        var policyRequiringAuthenticatedUser = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
                        options.Filters.Add(new AuthorizeFilter(policyRequiringAuthenticatedUser));
                        services.AddScoped<ISubManagementService, SubManagementService>();
                    }
                    else
                    {
                        services.AddScoped<ISubManagementService, FakeSubManagementService>();
                    }
                });

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

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc(_version, new OpenApiInfo {Title = _politikerafregningApi, Version = _version});
            });
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IGetTravelExpenseService, GetTravelExpenseService>();
            services.AddScoped<ICreateTravelExpenseService, CreateTravelExpenseService>();
            services.AddScoped<IUpdateTravelExpenseService, UpdateTravelExpenseService>();
            services.AddScoped<IProcessStepTravelExpenseService, ProcessStepTravelExpenseService>();

            Assembly
                .GetAssembly(typeof(IProcessFlowStep))
                .GetTypesAssignableFrom<IProcessFlowStep>()
                .ForEach(t => { services.AddScoped(typeof(IProcessFlowStep), t); });

            services.AddScoped<IHandle<TravelExpenseUpdatedDomainEvent>, TravelExpenseUpdatedNotificationHandler>();
            services.AddScoped<ISubManagementService, SubManagementService>();

            services.AddMvc();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger,
            IConfiguration configuration)
        {
            logger.Information("------------------------------------------------------------");
            logger.Information("Starting Politikerafregning Web API...");
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
                c => c.SwaggerEndpoint("/swagger/v1/swagger.json", _politikerafregningApi + " " + _version));

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