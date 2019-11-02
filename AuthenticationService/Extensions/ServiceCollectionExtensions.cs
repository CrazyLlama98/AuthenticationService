using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AuthenticationService.Models;
using Microsoft.AspNetCore.Identity;
using AuthenticationService.DbContexts;
using System.Reflection;
using AuthenticationService.Utilities;
using AutoMapper;
using AuthenticationService.Services.Interfaces;
using AuthenticationService.Services.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace AuthenticationService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, string connectionString)
        {
            return services
                .AddDbContext<AuthenticationDbContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                });
        }

        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<User, IdentityRole<long>>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<AuthenticationDbContext>()
                .AddDefaultTokenProviders();
            return services;
        }

        public static IServiceCollection AddConfiguredIdentityServer(
            this IServiceCollection services,
            string operationalConnection)
        {
            var migrationAssembly = typeof(ServiceCollectionExtensions).GetTypeInfo().Assembly.GetName().Name;

            services
                .AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
                .AddInMemoryClients(Configuration.GetClients())
                .AddAspNetIdentity<User>()
                .AddOperationalStore(options =>
                    options.ConfigureDbContext = builder =>
                        builder.UseNpgsql(operationalConnection, db => db.MigrationsAssembly(migrationAssembly)));
            return services;
        }

        public static IServiceCollection AddConfiguredMapper(this IServiceCollection services)
        {
            return services.AddAutoMapper(typeof(Startup).GetTypeInfo().Assembly);
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IAccountService, AccountService>();
        }

        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            return services
                .AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthenticationService", Version = "v1" });
                });
        }

        public static IServiceCollection AddCustomApiVersions(this IServiceCollection services)
        {
            return services
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                });
        }
    }
}
