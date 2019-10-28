using AuthenticationService.Dtos;
using AuthenticationService.Models;
using AutoMapper;

namespace AuthenticationService.MapperProfiles
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
