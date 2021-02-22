using Basic.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Basic
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {


         
            services.AddAuthorization(config =>
            {
                //var defaultauthbuilder = new authorizationpolicybuilder();
                //var defaultauthpolicy = defaultauthbuilder
                //.requireauthenticateduser()
                //.build();

                //config.AddPolicy("Claim.DoB",policyBuilder=>
                //{
                //    policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                //});
                //
                config.AddPolicy("Admin", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Role, "Role"));

                config.AddPolicy("Claim.DoB", policyBuilder =>
                {
                    policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
                });
            });

            services.AddScoped<IAuthorizationHandler,CustomRequireClaimHandler>(); 

            services.AddControllersWithViews();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
             
            app.UseAuthorization();
  
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
