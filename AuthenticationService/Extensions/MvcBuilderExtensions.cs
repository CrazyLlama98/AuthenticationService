using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Extensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder ConfigureApiBadRequestErrors(this IMvcBuilder mvcBuilder)
        {
            return mvcBuilder
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory =
                        actionContext => new BadRequestObjectResult(new ValidationErrorDetailsDto { Errors = new ValidationErrorDictionary(actionContext.ModelState) });
                });
        }
    }
}
