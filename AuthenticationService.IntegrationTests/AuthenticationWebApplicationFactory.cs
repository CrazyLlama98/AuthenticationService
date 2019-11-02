using System;
using System.IO;
using System.Reflection;
using AuthenticationService.DbContexts;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.IntegrationTests
{
    public class AuthenticationWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup>, IDisposable
        where TStartup : Startup
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var path = Assembly.GetAssembly(typeof(AuthenticationWebApplicationFactory<TStartup>)).Location;

            builder
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder
                        .AddJsonFile("appsettings.json", false)
                        .AddEnvironmentVariables();
                });
        }

        public new void Dispose()
        {
            using (var scope = Server.Host.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var authenticationDbContext = scope.ServiceProvider.GetService<AuthenticationDbContext>();
                var operationalDbContext = scope.ServiceProvider.GetService<PersistedGrantDbContext>();

                authenticationDbContext.Database.EnsureDeleted();
                operationalDbContext.Database.EnsureDeleted();
            }

            base.Dispose();
        }
    }
}
