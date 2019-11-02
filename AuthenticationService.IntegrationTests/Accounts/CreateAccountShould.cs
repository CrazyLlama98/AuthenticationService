using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService.Dtos;
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
        private readonly HttpClient _client;
        private readonly AuthenticationWebApplicationFactory<Startup> _factory;

        public CreateAccountShould(
            AuthenticationWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions {AllowAutoRedirect = false});
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
            var result = await _client.PostAsync(
                UrlHelper.Post.CreateAccount,
                new StringContent(JsonConvert.SerializeObject(accountDto), Encoding.UTF8, "application/json"));

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
