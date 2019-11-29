using AuthenticationService.Infrastructure.DbContexts;
using AuthenticationService.Domain.Models;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using AuthenticationService.Utilities;
using System.Linq;
using IdentityServer4.EntityFramework.Mappers;

namespace AuthenticationService.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigrations(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var authenticationDbContext = scope.ServiceProvider.GetService<AuthenticationDbContext>();
                var operationalDbContext = scope.ServiceProvider.GetService<PersistedGrantDbContext>();
                var configurationDbContext = scope.ServiceProvider.GetService<ConfigurationDbContext>();

                authenticationDbContext.Database.Migrate();
                operationalDbContext.Database.Migrate();
                configurationDbContext.Database.Migrate();
            }

            return app;
        }

        public static IApplicationBuilder UsePostmanClient(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var configurationDbContext = scope.ServiceProvider.GetService<ConfigurationDbContext>();

                if (!configurationDbContext.Clients.Any())
                {
                    configurationDbContext.Clients.AddRange(Configuration.GetClients().Select(client => client.ToEntity()));
                    configurationDbContext.SaveChanges();
                }

                if (!configurationDbContext.IdentityResources.Any())
                {
                    configurationDbContext.IdentityResources.AddRange(Configuration.GetIdentityResources().Select(resource => resource.ToEntity()));
                    configurationDbContext.SaveChanges();
                }

                if (!configurationDbContext.ApiResources.Any())
                {
                    configurationDbContext.ApiResources.AddRange(Configuration.GetApiResources().Select(resource => resource.ToEntity()));
                    configurationDbContext.SaveChanges();
                }
            }

            return app;
        }

        public static IApplicationBuilder UseAdminAccount(
            this IApplicationBuilder app,
            string adminUsername,
            string adminPassword,
            string adminRoleName,
            string adminEmail)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole<long>>>();
                var userManager = scope.ServiceProvider.GetService<UserManager<User>>();

                if (!roleManager.RoleExistsAsync(adminRoleName).Result)
                {
                    roleManager.CreateAsync(new IdentityRole<long>(adminRoleName)).Wait();
                }

                if (userManager.FindByNameAsync(adminUsername).Result == null)
                {
                    var admin = new User { UserName = adminUsername, Email = adminEmail };
                    var result = userManager.CreateAsync(admin, adminPassword).Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(admin, adminRoleName);
                    }
                }
            }

            return app;
        }

        public static IApplicationBuilder UseApplicationRoles(this IApplicationBuilder app, IEnumerable<string> roles)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole<long>>>();

                foreach (var role in roles)
                {
                    if (!roleManager.RoleExistsAsync(role).Result)
                    {
                        roleManager.CreateAsync(new IdentityRole<long>(role)).Wait();
                    }
                }
            }

            return app;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            return app
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthenticationService");
                });
        }
    }
}
