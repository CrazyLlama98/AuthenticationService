using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AuthenticationService.Domain.Models;
using Microsoft.AspNetCore.Identity;
using AuthenticationService.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace AuthenticationService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            return services
                .AddCors(options =>
                {
                    options.AddPolicy("AllowAll",
                        builder => builder
                            .WithOrigins("http://localhost:8080")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());
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
            string operationalConnection,
            string configurationConnection)
        {
            services
                .AddIdentityServer(options =>
                {
                    options.IssuerUri = "http://test.identityserver.com";
                    options.Endpoints.EnableJwtRequestUri = true;
                })
                .AddAspNetIdentity<User>()
                .AddOperationalStore(options =>
                    options.ConfigureDbContext = builder =>
                        builder.UseNpgsql(operationalConnection, dbOptions => dbOptions.MigrationsAssembly(@"AuthenticationService.Infrastructure")))
                .AddConfigurationStore(options => 
                    options.ConfigureDbContext = builder => 
                        builder.UseNpgsql(configurationConnection, dbOptions => dbOptions.MigrationsAssembly(@"AuthenticationService.Infrastructure")))
                .AddDeveloperSigningCredential();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/login";
            });

            return services;
        }

        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            return services
                .AddSwaggerGen(options =>
                {
                    options.EnableAnnotations();
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
