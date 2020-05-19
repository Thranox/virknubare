using System;
using System.Linq;
using IdentityServerAspNetIdentit.Data;
using IdentityServerAspNetIdentit.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SharedWouldBeNugets;

namespace IDP
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    var pendingMigrations = context.Database.GetPendingMigrations();
                    foreach (var pendingMigration in pendingMigrations)
                        context
                            .Database.GetInfrastructure()
                            .GetService<IMigrator>()
                            .MigrateAsync(pendingMigration)
                            .Wait();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    GetOrCreateUser(userMgr, "alice");
                    GetOrCreateUser(userMgr, "bob");
                    GetOrCreateUser(userMgr, "charlie");
                    GetOrCreateUser(userMgr, "dennis");
                    GetOrCreateUser(userMgr, "edward");
                }
            }
        }

        private static void GetOrCreateUser(UserManager<ApplicationUser> userMgr, string userName)
        {
            var user = userMgr.FindByNameAsync(userName).Result;
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = userName
                };
                var result = userMgr
                    .CreateAsync(user, "Pass123$")
                    .Result;
                
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                result = userMgr
                    .AddClaimsAsync(user, TestData.GetClaimsByUserName(userName))
                    .Result;
                
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                Log.Debug(userName + " created");
            }
            else
            {
                Log.Debug(userName + " already exists");
            }
        }
    }
}