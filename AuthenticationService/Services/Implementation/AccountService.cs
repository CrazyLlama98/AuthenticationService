using System.Linq;
using System.Threading.Tasks;
using AuthenticationService.Dtos;
using AuthenticationService.Models;
using AuthenticationService.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AccountService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<OperationResultDto> CreateAccountAsync(AccountDto accountDto)
        {
            var user = _mapper.Map<User>(accountDto);

            var result = await _userManager.CreateAsync(user, accountDto.Password);

            return result.Succeeded ? CreateSuccessfulResult() : CreateResultWithError(result);
        }

        private static OperationResultDto CreateResultWithError(IdentityResult result)
        {
            return new OperationResultDto { IsSuccessful = false, Errors = result.Errors.Select(error => error.Description) };
        }

        private static OperationResultDto CreateSuccessfulResult()
        {
            return new OperationResultDto { IsSuccessful = true };
        }
    }
}
