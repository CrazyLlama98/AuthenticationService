using AuthenticationService.Business.Implementation;
using AuthenticationService.Business.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Business.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<IApiResourceService, ApiResourceService>();
        }
    }
}
