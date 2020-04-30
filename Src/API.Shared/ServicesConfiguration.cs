using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Threading.Tasks;
using API.Shared.ActionFilters;
using API.Shared.Controllers;
using API.Shared.Services;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Interfaces;
using Domain.SharedKernel;
using IdentityServer4.AccessTokenValidation;
using Infrastructure.Data;
using Infrastructure.DomainEvents;
using Infrastructure.DomainEvents.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using SharedWouldBeNugets;
using Web.MapperProfiles;

namespace API.Shared
{
    public static class ServicesConfiguration
    {
        public static void AddPolApi(this IServiceCollection services, IConfiguration configuration, bool enforceAuthenticated)
        {
            services.AddSingleton<ILogger>(Log.Logger);

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration.GetValue<string>("IDP_URL");
                    options.ApiName = "teapi";
                    options.RequireHttpsMetadata = false;


                    Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
                })
                ;
            var assembly = typeof(TravelExpenseController).Assembly;
            services.AddControllersWithViews(options =>
                {
                    // Include handling of Domain Exceptions
                    options.Filters.Add(new HttpResponseExceptionFilter(Log.Logger));
                    // Log all entries and exits of controller methods.
                    options.Filters.Add(new MethodLoggingActionFilter(Log.Logger));
                    // Find user for request

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
                x.SwaggerDoc(CommonApi.Version, new OpenApiInfo { Title = CommonApi.Title, Version = CommonApi.Version });
            });

            MapServices(services, enforceAuthenticated);

            services.AddMvc(mvcOptions => mvcOptions.EnableEndpointRouting = false);

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public static void MapServices(IServiceCollection services, bool enforceAuthenticated)
        {
            services.AddAutoMapper(typeof(EntityDtoProfile));

            // Infrastructure
            services.AddScoped<IRepository, EfRepository>();
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            // Application services
            services.AddScoped<IGetTravelExpenseService, GetTravelExpenseService>();
            services.AddScoped<ICreateTravelExpenseService, CreateTravelExpenseService>();
            services.AddScoped<IUpdateTravelExpenseService, UpdateTravelExpenseService>();
            services.AddScoped<IProcessStepTravelExpenseService, ProcessStepTravelExpenseService>();
            services.AddScoped<IGetFlowStepService, GetFlowStepService>();
            services.AddScoped<ICreateSubmissionService, CreateSubmissionService>();

            if (enforceAuthenticated)
            {
                services.AddScoped<ISubManagementService, SubManagementService>();
            }
            else
            {
                services.AddScoped<ISubManagementService, FakeSubManagementService>();
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