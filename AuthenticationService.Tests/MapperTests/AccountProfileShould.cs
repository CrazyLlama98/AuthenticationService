using AuthenticationService.Dtos;
using AuthenticationService.Models;
using AuthenticationService.Test.MapperTests.DataSets;
using AutoMapper;
using FluentAssertions;
using System.Reflection;
using Xunit;

namespace AuthenticationService.Test.MapperTests
{
    public class AccountProfileShould
    {
        private readonly IMapper _mapper;

        public AccountProfileShould()
        {
            _mapper = new MapperConfiguration(options => options.AddMaps(typeof(Startup).GetTypeInfo().Assembly)).CreateMapper();
        }

        [Theory]
        [ClassData(typeof(AccountDtoToUserTestDataSet))]
        public void MapAccountDtoToUser(AccountDto accountDto, User expected)
        {
            // act
            var result = _mapper.Map<User>(accountDto);

            // assert
            result.Email.Should().Be(expected.Email);
            result.UserName.Should().Be(expected.UserName);
        }
    }
}
