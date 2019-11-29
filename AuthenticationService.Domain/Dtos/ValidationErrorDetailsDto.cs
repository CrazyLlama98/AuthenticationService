using AuthenticationService.Domain.Common;

namespace AuthenticationService.Domain.Dtos
{
    public class ValidationErrorDetailsDto
    {
        public ValidationErrorDictionary Errors { get; set; }
    }
}
