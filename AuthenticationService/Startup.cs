using AuthenticationService.Business.Extensions;
using AuthenticationService.Extensions;
using AuthenticationService.Utilities;
using FluentValidation.AspNetCore;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace AuthenticationService
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
            services
                .AddCustomCors()
                .AddApplicationDbContext(Configuration.GetConnectionString(Constants.AuthenticationConnectionStringKey))
                .AddCustomIdentity()
                .AddConfiguredIdentityServer(
                    Configuration.GetConnectionString(Constants.OperationalConnectionStringKey),
                    Configuration.GetConnectionString(Constants.ConfigurationConnectionStringKey))
                .AddConfiguredMapper()
                .AddValidators()
                .AddUnitOfWork()
                .AddRepositories()
                .AddApplicationServices()
                .AddSwaggerDocumentation()
                .AddCustomApiVersions()
                .AddMvc()
                .ConfigureApiBadRequestErrors()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                })
                .AddFluentValidation()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    // base-address of your identityserver
                    options.Authority = "http://test.identityserver.com";

                    // name of the API resource
                    options.ApiName = "auth-service";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseDatabaseMigrations();
            app.UseAdminAccount(Configuration["Admin:Username"], Configuration["Admin:Password"], Configuration["Admin:Role"], Configuration["Admin:Email"])
                .UseApplicationRoles(Configuration.GetSection(Constants.ApplicationRolesKey).Get<IEnumerable<string>>());
            app.UseSwaggerDocumentation();
            app.UseIdentityServer();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

            // remove after ui is done
            app.UsePostmanClient();
        }
    }
}
