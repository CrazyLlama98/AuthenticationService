using AuthenticationService.Extensions;
using AuthenticationService.Utilities;
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
                .AddApplicationDbContext(Configuration.GetConnectionString(Constants.AuthenticationConnectionStringKey))
                .AddCustomIdentity()
                .AddConfiguredIdentityServer(Configuration.GetConnectionString(Constants.OperationalConnectionStringKey))
                .AddConfiguredMapper()
                .AddApplicationServices()
                .AddSwaggerDocumentation()
                .AddCustomApiVersions()
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDatabaseMigrations();
            app.UseAdminAccount(Configuration["Admin:Username"], Configuration["Admin:Password"], Configuration["Admin:Role"])
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
        }
    }
}
