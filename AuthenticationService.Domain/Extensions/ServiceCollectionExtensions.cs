using AuthenticationService.Domain.Dtos;
using AuthenticationService.Domain.Validators;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AuthenticationService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguredMapper(this IServiceCollection services)
        {
            return services.AddAutoMapper(typeof(ServiceCollectionExtensions).GetTypeInfo().Assembly);
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services
                .AddTransient<IValidator<AccountDto>, AccountDtoValidator>();
        }
    }
}
