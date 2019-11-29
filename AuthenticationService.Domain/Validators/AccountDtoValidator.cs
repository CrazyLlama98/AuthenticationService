using AuthenticationService.Domain.Dtos;
using FluentValidation;

namespace AuthenticationService.Domain.Validators
{
    public class AccountDtoValidator : AbstractValidator<AccountDto>
    {
        public AccountDtoValidator()
        {
            RuleFor(account => account.Email)
                .NotNull()
                .EmailAddress();
            
            RuleFor(account => account.Password)
                .NotNull()
                .MinimumLength(8);
            
            RuleFor(account => account.Username)
                .NotNull()
                .NotEmpty();
        }
    }
}
