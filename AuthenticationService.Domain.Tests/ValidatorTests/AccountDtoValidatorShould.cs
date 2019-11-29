using AuthenticationService.Domain.Validators;
using Xunit;
using FluentValidation.TestHelper;

namespace AuthenticationService.Domain.Tests.ValidatorTests
{
    public class AccountDtoValidatorShould
    {
        private readonly AccountDtoValidator validator = new AccountDtoValidator();

        [Fact]
        public void HaveErrorWhenEmailIsNull()
        {
            // assert
            validator.ShouldHaveValidationErrorFor(account => account.Email, null as string);
        }

        [Theory]
        [InlineData("")]
        [InlineData("sgnyenyjej")]
        [InlineData("geehe.com")]
        public void HaveErrorWhenEmailIsNotValid(string email)
        {
            // assert
            validator.ShouldHaveValidationErrorFor(account => account.Email, email);
        }

        [Theory]
        [InlineData("avg@rw.com")]
        [InlineData("sgnyenyjej@gmail.com")]
        [InlineData("gee45_h@cre.com")]
        public void NotHaveErrorWhenEmailIsValid(string email)
        {
            // assert
            validator.ShouldNotHaveValidationErrorFor(account => account.Email, email);
        }

        [Fact]
        public void HaveErrorWhenUsernameIsNull()
        {
            // assert
            validator.ShouldHaveValidationErrorFor(account => account.Username, null as string);
        }

        [Fact]
        public void HaveErrorWhenUsernameIsEmpty()
        {
            // assert
            validator.ShouldHaveValidationErrorFor(account => account.Username, "");
        }

        [Fact]
        public void NotHaveErrorWhenUsernameIsNotNull()
        {
            // assert
            validator.ShouldNotHaveValidationErrorFor(account => account.Username, "test");
        }

        [Fact]
        public void HaveErrorWhenPasswordIsNull()
        {
            // assert
            validator.ShouldHaveValidationErrorFor(account => account.Password, null as string);
        }

        [Theory]
        [InlineData("")]
        [InlineData("fve446")]
        [InlineData("rhh433!")]
        public void HaveErrorWhenPasswordIsShorterMinLength(string password)
        {
            // assert
            validator.ShouldHaveValidationErrorFor(account => account.Password, password);
        }

        [Theory]
        [InlineData("46h4yeaB#%5#")]
        [InlineData("12345678")]
        [InlineData("53htbh3h(^*^%hvgg")]
        public void NotHaveErrorWhenPasswordIsLongerMinLength(string password)
        {
            // assert
            validator.ShouldNotHaveValidationErrorFor(account => account.Password, password);
        }
    }
}
