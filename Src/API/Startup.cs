using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:44305";
                    options.ApiName = "teapi";
                    options.RequireHttpsMetadata = false;
                });
 
            
            
            //services.AddAuthentication(options =>
            //    {
            //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    }).AddJwtBearer(options =>
            //    {
            //        options.Authority = "https://localhost:44305";
            //        options.Audience = "teapi";
            //        options.RequireHttpsMetadata = false;
            //    })
            //    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            //    {
            //        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //        options.Authority = "https://localhost:44305/";
            //        options.ClientId = "polangularclient";
            //        options.ResponseType = "code";
            //        options.Scope.Add("roles");
            //        options.ClaimActions.MapUniqueJsonKey("role","role");
            //        options.GetClaimsFromUserInfoEndpoint = true;
            //        });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("TeCreator", policy => policy.RequireClaim(ClaimTypes.Role, "tecreator"));
                options.AddPolicy("TeCertifier", policy => policy.RequireClaim(ClaimTypes.Role, "tecertifier"));
            });

            services.AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
            //services.AddTransient<IProfileService, MyProfileService>();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}