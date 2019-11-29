using AuthenticationService.Domain.Dtos;
using AuthenticationService.Domain.Models;
using AutoMapper;

namespace AuthenticationService.Domain.MapperProfiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<AccountDto, User>()
                .ForMember(user => user.Email, opt => opt.MapFrom(accountDto => accountDto.Email))
                .ForMember(user => user.UserName, opt => opt.MapFrom(accountDto => accountDto.Username));
        }
    }
}
