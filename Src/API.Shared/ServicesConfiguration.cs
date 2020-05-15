using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using API.Shared.ActionFilters;
using API.Shared.Controllers;
using API.Shared.Services;
using Application.Interfaces;
using Application.MapperProfiles;
using Application.Services;
using AutoMapper;
using Domain;
using Domain.Events;
using Domain.Interfaces;
using Domain.Services;
using IdentityServer4.AccessTokenValidation;
using IDP.Services;
using Infrastructure.Data;
using Infrastructure.DomainEvents;
using Infrastructure.DomainEvents.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using SharedWouldBeNugets;

namespace API.Shared
{
    public static class ServicesConfiguration
    {
        public static void AddPolDatabase(this IServiceCollection services, IConfiguration configuration,
            string environmentName)
        {
            services.AddScoped<IPolicyService, PolicyService>();

            services.AddDbContext<PolDbContext>(options =>
            {
                var connectionStringService = new ConnectionStringService(configuration, environmentName);
                var connectionString = connectionStringService.GetConnectionString("PolConnection");
                options.UseSqlServer(connectionString);
            });

        }
        public static void AddPolApi(this IServiceCollection services, IConfiguration configuration, bool enforceAuthenticated, string apiTitle, string componentName)
        {
            services.AddControllers();
            services.AddScoped<HttpResponseExceptionFilter>();
            services.AddScoped<MethodLoggingActionFilter>();
            services.AddScoped<ILogger>(s=> StartupHelper.CreateLogger(configuration, componentName));

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration.GetValue<string>("IDP_URL");
                    options.ApiName = "teapi";
                    options.RequireHttpsMetadata = false;
                    options.ApiSecret = "secret";


                    Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
                });

            var assembly = typeof(TravelExpenseController).Assembly;
            services.AddControllersWithViews(options =>
                {
                    // Include handling of Domain Exceptions
                    options.Filters.Add<HttpResponseExceptionFilter>();
                    // Log all entries and exits of controller methods.
                    options.Filters.Add<MethodLoggingActionFilter>();

                    // If desired, be set up a global Authorize filter
                    if (enforceAuthenticated)
                    {
                        var policyRequiringAuthenticatedUser = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
                        options.Filters.Add(new AuthorizeFilter(policyRequiringAuthenticatedUser));
                    }
                })
                .AddApplicationPart(assembly);

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc(CommonApi.Version, new OpenApiInfo { Title = apiTitle, Version = CommonApi.Version });
            });

            MapServices(services, enforceAuthenticated, configuration);

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public static void MapServices(IServiceCollection services, bool enforceAuthenticated, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(EntityDtoProfile));

            // Infrastructure
            services.AddScoped<IRepository, EfRepository>();
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDbSeeder, DbSeeder>();

            // Application services
            services.AddScoped<IGetTravelExpenseService, GetTravelExpenseService>();
            services.AddScoped<ICreateTravelExpenseService, CreateTravelExpenseService>();
            services.AddScoped<IUpdateTravelExpenseService, UpdateTravelExpenseService>();
            services.AddScoped<IProcessStepTravelExpenseService, ProcessStepTravelExpenseService>();
            services.AddScoped<IGetFlowStepService, GetFlowStepService>();
            services.AddScoped<ICreateSubmissionService, CreateSubmissionService>();
            services.AddScoped<IGetStatisticsService, GetStatisticsService>();
            services.AddScoped<IUserCustomerStatusService, UserCustomerStatusService>();
            services.AddScoped<IGetUserInfoService, GetUserInfoService>();

            services.AddScoped<ICreateCustomerService, CreateCustomerService>();
            services.AddScoped<ICreateUserService, CreateUserService>();
            services.AddScoped<ITravelExpenseFactory, TravelExpenseFactory>();
            services.AddScoped<IStageService, StageService>();

            if (enforceAuthenticated)
            {
                services.AddScoped<ISubManagementService, SubManagementService>();
            }
            else
            {
                services.AddScoped<ISubManagementService>(x=>new FakeSubManagementService(configuration.GetValue<string>("SubUsedWhenAuthenticationDisabled")) );
            }

            Assembly
                .GetAssembly(typeof(IProcessFlowStep))
                .GetTypesAssignableFrom<IProcessFlowStep>()
                .ForEach(t => { services.AddScoped(typeof(IProcessFlowStep), t); });

            // Domain event handlers
            services.AddScoped<IHandle<TravelExpenseUpdatedDomainEvent>, TravelExpenseUpdatedNotificationHandler>();
        }
    }
}