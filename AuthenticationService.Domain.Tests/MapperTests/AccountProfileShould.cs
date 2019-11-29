using AuthenticationService.Domain.Dtos;
using AuthenticationService.Domain.Models;
using AuthenticationService.Domain.Tests.MapperTests.DataSets;
using AutoMapper;
using FluentAssertions;
using System.Reflection;
using Xunit;

namespace AuthenticationService.Domain.Tests.MapperTests
{
    public class AccountProfileShould
    {
        private readonly IMapper _mapper;

        public AccountProfileShould()
        {
            _mapper = new MapperConfiguration(options => options.AddMaps(typeof(MapperProfiles.AccountProfile).GetTypeInfo().Assembly)).CreateMapper();
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
