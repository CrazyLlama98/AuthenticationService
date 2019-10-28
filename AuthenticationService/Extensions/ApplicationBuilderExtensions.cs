using AuthenticationService.DbContexts;
using AuthenticationService.Models;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

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

                authenticationDbContext.Database.Migrate();
                operationalDbContext.Database.Migrate();
            }

            return app;
        }

        public static IApplicationBuilder UseAdminAccount(
            this IApplicationBuilder app,
            string adminUsername,
            string adminPassword,
            string adminRoleName)
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
                    userManager.CreateAsync(new User { UserName = adminUsername, Email = adminUsername }, adminPassword).Wait();
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
