using AuthenticationService.Domain.Repositories;
using AuthenticationService.Domain.UnitOfWork;
using AuthenticationService.Infrastructure.DbContexts;
using AuthenticationService.Infrastructure.Repositories;
using AuthenticationService.Infrastructure.UnitOfWork;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, string connectionString)
        {
            return services
                .AddDbContext<AuthenticationDbContext>(options =>
                {
                    options.UseNpgsql(connectionString, dbOptions => dbOptions.MigrationsAssembly(@"AuthenticationService.Infrastructure"));
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            return services
                .AddScoped<IUnitOfWork<ConfigurationDbContext>, UnitOfWork<ConfigurationDbContext>>();
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped<IApiResourceRepository, ApiResourceRepository>();
        }
    }
}
