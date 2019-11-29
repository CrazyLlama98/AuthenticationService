using AuthenticationService.Domain.Dtos;
using AuthenticationService.Domain.Models;
using AuthenticationService.Business.Implementation;
using AuthenticationService.Business.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Reflection;
using Xunit;
using System.Threading.Tasks;
using FluentAssertions;

namespace AuthenticationService.Business.Tests.ServicesTests
{
    public class AccountServiceShould
    {
        private readonly IAccountService _accountService;
        private readonly Mock<TestUserManager> _userManager;
        private readonly IMapper _mapper;

        public AccountServiceShould()
        {
            _userManager = new Mock<TestUserManager>();
            _mapper = new MapperConfiguration(options => options.AddMaps(typeof(Domain.MapperProfiles.AccountProfile).GetTypeInfo().Assembly)).CreateMapper();

            _accountService = new AccountService(_userManager.Object, _mapper);
        }

        [Fact]
        public async Task ReturnValidOperationResultWhenUserIsCreated()
        {
            // arrange
            var account = new AccountDto { Username = "test123", Email = "test1@test.com", Password = "test123" };
            _userManager.Setup(userManager => userManager.CreateAsync(It.IsAny<User>(), account.Password))
                .ReturnsAsync(IdentityResult.Success);

            // act
            var result = await _accountService.CreateAccountAsync(account);

            // assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public async Task ReturnInvalidOperationResultWithErrorsWhenUserCreationFails()
        {
            // arrange
            var account = new AccountDto { Username = "test123", Email = "test1@test.com", Password = "test123" };
            _userManager.Setup(userManager => userManager.CreateAsync(It.IsAny<User>(), account.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "101", Description = "Test error" }));

            // act
            var result = await _accountService.CreateAccountAsync(account);

            // assert
            result.IsSuccessful.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
        }
    }
}
