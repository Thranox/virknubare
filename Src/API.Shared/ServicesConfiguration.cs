using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using API.Shared.ActionFilters;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
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
            services.AddScoped<HttpResponseExceptionFilter>();
            services.AddScoped<MethodLoggingActionFilter>();
            services.AddScoped<ILogger>(s => StartupHelper.CreateLogger(configuration, componentName));

            services.AddControllers().AddNewtonsoftJson();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // base-address of your identityserver
                    options.Authority = configuration.GetValue<string>("IDP_URL");

                    // name of the API resource
                    options.Audience = "teapi";
                    options.RequireHttpsMetadata = false;
                    Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
                });

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
}