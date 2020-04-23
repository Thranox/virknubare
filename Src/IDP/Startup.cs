// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IDP.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace IDP
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            _configuration = configuration;
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                try
                {
                    var userIdentityDbContext = serviceScope.ServiceProvider.GetRequiredService<UserIdentityDbContext>();
                    userIdentityDbContext.Database.EnsureCreated();
                    userIdentityDbContext.Database.Migrate();
                }
                catch (Exception e)
                {
                    Log.Logger.Error(e, "During Creating/Migrating UserIdentityDbContext");
                    throw;
                }

                try
                {
                    Log.Logger.Information("Creating/Migrating PersistedGrantDbContext");
                    var persistedGrantDbContext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
                    //persistedGrantDbContext.Database.EnsureCreated();
                    //persistedGrantDbContext.Database.Migrate();
                }
                catch (Exception e)
                {
                    Log.Logger.Error(e, "During Creating/Migrating PersistedGrantDbContext");
                    throw;
                }

                ConfigurationDbContext context;
                try
                {
                    Log.Logger.Information("Creating/Migrating ConfigurationDbContext");
                    context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                    //context.Database.EnsureCreated();
                    //context.Database.Migrate();
                }
                catch (Exception e)
                {
                    Log.Logger.Error(e, "During Creating/Migrating ConfigurationDbContext");
                    throw;
                }

                if (!context.Clients.Any())
                {
                    foreach (var client in Config.Clients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.Ids)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.Apis)
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger.Information("ConfigureServices()");

            services.AddTransient<IConnectionStringService, ConnectionStringService>();

            // Basic webapp with pages
            services.AddMvc();
            services.AddControllersWithViews();
            services.AddRazorPages();

            // ASP.NET Core Identity
            services.AddDbContext<UserIdentityDbContext>(options =>
            {
                var connectionStringService =services.BuildServiceProvider().GetRequiredService<IConnectionStringService>();
                var conUserIdentityDbContext = connectionStringService.GetConnectionString("AspNetCoreIdentity");
                options.UseSqlServer(conUserIdentityDbContext);
            });
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<UserIdentityDbContext>()
                .AddDefaultTokenProviders()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddSignInManager<SignInManager<ApplicationUser>>();

            services.AddTransient<IEmailSender, EmailSenderService>();

            // IdentityServer4
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddTestUsers(TestUsers.Users)
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    var connectionStringService = services.BuildServiceProvider().GetRequiredService<IConnectionStringService>();
                    var conIdentityServer4 = connectionStringService.GetConnectionString("IdentityServer4");
                    
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(conIdentityServer4,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    var connectionStringService = services.BuildServiceProvider().GetRequiredService<IConnectionStringService>();
                    var conIdentityServer4 = connectionStringService.GetConnectionString("IdentityServer4");
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(conIdentityServer4,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    //options.EnableTokenCleanup = true;
                    //options.TokenCleanupInterval = 30;
                })
                .AddAspNetIdentity<ApplicationUser>()
                // not recommended for production - you need to store your key material somewhere secure
                .AddDeveloperSigningCredential();

            services.AddCors(setup =>
            {
                setup.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.WithOrigins("https://localhost:44324");
                    policy.AllowCredentials();
                });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            Log.Logger.Information("Configure()");

            InitializeDatabase(app);

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
