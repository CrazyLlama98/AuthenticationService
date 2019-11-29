using System;
using System.Net;
using System.Threading.Tasks;
using AuthenticationService.Domain.Dtos;
using AuthenticationService.Domain.Extensions;
using AuthenticationService.IntegrationTests.Utilities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace AuthenticationService.IntegrationTests.Accounts
{
    public class CreateAccountShould
        : IClassFixture<AuthenticationWebApplicationFactory<Startup>>
    {
        private readonly RequestHelper _requestHelper;

        public CreateAccountShould(
            AuthenticationWebApplicationFactory<Startup> factory)
        {
            var client = factory.CreateClient(new WebApplicationFactoryClientOptions {AllowAutoRedirect = false});
            _requestHelper = new RequestHelper(client);
        }

        [Fact]
        public async Task ReturnNoContentWhenAccountIsCreated()
        {
            // arrange
            var accountDto = new AccountDto
            {
                Email = "Test@test.com", Password = "Test1234", Username = Guid.NewGuid().ToString()
            };

            // act
            var result = await _requestHelper.PostObject(UrlHelper.Post.CreateAccount, accountDto);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("", "", "")]
        [InlineData("tg3y4y", "134", "")]
        
        public async Task ReturnBadRequestIfModelStateValidationFails(string email, string password, string username)
        {
            // arrange
            var accountDto = new AccountDto
            {
                Email = email,
                Password = password,
                Username = username
            };

            // act
            var result = await _requestHelper.PostObject(UrlHelper.Post.CreateAccount, accountDto);
            var validationErrorDetails = JsonConvert.DeserializeObject<ValidationErrorDetailsDto>(await result.Content.ReadAsStringAsync());

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            validationErrorDetails.Errors[nameof(accountDto.Email).ToCamelCaseFromPascalCase()].Should().NotBeNull();
            validationErrorDetails.Errors[nameof(accountDto.Username).ToCamelCaseFromPascalCase()].Should().NotBeNull();
            validationErrorDetails.Errors[nameof(accountDto.Password).ToCamelCaseFromPascalCase()].Should().NotBeNull();
        }

        [Fact]
        public async Task ReturnBadRequestIfUsernameAlreadyExists()
        {
            // arrange
            var existingUserAccount = new AccountDto { Email = "test1@test.com", Username = "test1", Password = "test1234" };
            await _requestHelper.PostObject(UrlHelper.Post.CreateAccount, existingUserAccount);
            var accountDto = new AccountDto { Email = "test2@test.com", Password = "test1234", Username = "test1" };

            // act
            var result = await _requestHelper.PostObject(UrlHelper.Post.CreateAccount, accountDto);
            var validationErrorDetails = JsonConvert.DeserializeObject<ValidationErrorDetailsDto>(await result.Content.ReadAsStringAsync());

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            validationErrorDetails.Errors["default"].Should().NotBeNull();
        }

        [Fact]
        public async Task ReturnBadRequestIfEmailAlreadyExists()
        {
            // arrange
            var existingUserAccount = new AccountDto { Email = "test1@test.com", Username = "test1", Password = "test1234" };
            await _requestHelper.PostObject(UrlHelper.Post.CreateAccount, existingUserAccount);
            var accountDto = new AccountDto { Email = "test1@test.com", Password = "test1234", Username = "test2" };

            // act
            var result = await _requestHelper.PostObject(UrlHelper.Post.CreateAccount, accountDto);
            var validationErrorDetails = JsonConvert.DeserializeObject<ValidationErrorDetailsDto>(await result.Content.ReadAsStringAsync());

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            validationErrorDetails.Errors["default"].Should().NotBeNull();
        }
    }
}
