using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Threading.Tasks;
using API.Shared.ActionFilters;
using API.Shared.Controllers;
using API.Shared.Services;
using Application;
using Application.Interfaces;
using Application.MapperProfiles;
using Application.Services;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using Domain.Services;
using IdentityServer4.AccessTokenValidation;
using Infrastructure.Data;
using Infrastructure.DomainEvents;
using Infrastructure.DomainEvents.Handlers;
using Infrastructure.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
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
            services.AddControllers().AddNewtonsoftJson();
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
            //services.AddMvc(x =>
            //{
                
            //})
            //    .AddApplicationPart(assembly);
            //services.AddControllersWithViews(options =>
            //    {
            //        // Include handling of Domain Exceptions
            //        options.Filters.Add<HttpResponseExceptionFilter>();
            //        // Log all entries and exits of controller methods.
            //        options.Filters.Add<MethodLoggingActionFilter>();

            //        // If desired, be set up a global Authorize filter
            //        if (enforceAuthenticated)
            //        {
            //            var policyRequiringAuthenticatedUser = new AuthorizationPolicyBuilder()
            //                .RequireAuthenticatedUser()
            //                .Build();
            //            //TODO reenable
            //            //options.Filters.Add(new WrapperFilter( new AuthorizeFilter(policyRequiringAuthenticatedUser),StartupHelper.CreateLogger(configuration,componentName)));
            //        }
            //    })
            //    .AddApplicationPart(assembly);

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
            services.AddScoped<IFlowStepTravelExpenseService, FlowStepTravelExpenseService>();
            services.AddScoped<IGetFlowStepService, GetFlowStepService>();
            services.AddScoped<ICreateSubmissionService, CreateSubmissionService>();
            services.AddScoped<IGetStatisticsService, GetStatisticsService>();
            services.AddScoped<IUserCustomerStatusService, UserCustomerStatusService>();
            services.AddScoped<IGetUserInfoService, GetUserInfoService>();
            services.AddScoped<IAdminService, AdminService>();

            services.AddScoped<ICreateCustomerService, CreateCustomerService>();
            services.AddScoped<ICreateUserService, CreateUserService>();
            services.AddScoped<ITravelExpenseFactory, TravelExpenseFactory>();
            services.AddScoped<IStageService, StageService>();
            services.AddScoped<IMessageBrokerService, MessageBrokerService>();
            services.AddScoped<IMessageTemplateService, MessageTemplateService>();
            services.AddScoped<IMessageFactory, MessageFactory>();
            services.AddScoped<IUserStatusService, UserStatusService>();
            services.AddScoped<ICustomerUserService, CustomerUserService>();
            services.AddScoped<IInvitationService, InvitationService>();
            services.AddScoped<ISubmitSubmissionService, SubmitSubmissionService>();
            
            if (enforceAuthenticated)
            {
                services.AddScoped<ISubManagementService, SubManagementService>();
            }
            else
            {
                services.AddScoped<ISubManagementService>(x => new FakeSubManagementService(
                    new PolApiContext(
                        new UserEntity("Temp", configuration.GetValue<string>("SubUsedWhenAuthenticationDisabled")),
                        "http://nowhere.com",new PolSystem("http://nowhere.com/api", "http://nowhere.com/web")
                        )));
            }

            Assembly
                .GetAssembly(typeof(ProcessFlowStepAssignedForPaymentFinal))
                .GetTypesAssignableFrom<IProcessFlowStep>()
                .ForEach(t => { services.AddScoped(typeof(IProcessFlowStep), t); });

            Assembly
                .GetAssembly(typeof(EmailMessageSenderService))
                .GetTypesAssignableFrom<IMessageSenderService>()
                .ForEach(t => { services.AddScoped(typeof(IMessageSenderService), t); });

            // Domain event handlers
            services.AddScoped<IHandle<TravelExpenseUpdatedDomainEvent>, TravelExpenseUpdatedNotificationHandler>();
            services.AddScoped<IHandle<TravelExpenseChangedStateDomainEvent>, TravelExpenseChangedStateDomainEventHandler>();
            services.AddScoped<IHandle<InvitationAddedDomainEvent>, InvitationAddedDomainEventEventHandler>();
        }
    }

    public class WrapperFilter : IAsyncAuthorizationFilter, IFilterMetadata, IFilterFactory
    {
        private readonly AuthorizeFilter _authorizeFilter;
        private readonly ILogger _logger;

        public WrapperFilter(AuthorizeFilter authorizeFilter, ILogger logger)
        {
            _authorizeFilter = authorizeFilter;
            _logger = logger;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            _logger.Debug("Result: "+JsonConvert.SerializeObject( context.Result));
            await _authorizeFilter.OnAuthorizationAsync(context);
        }


        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return this;
        }

        public bool IsReusable { get; }
    }
}