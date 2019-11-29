using System.Linq;
using System.Threading.Tasks;
using AuthenticationService.Domain.Dtos;
using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.Models;
using AuthenticationService.Business.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.Business.Implementation
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

        public async Task<OperationResult> CreateAccountAsync(AccountDto accountDto)
        {
            var user = _mapper.Map<User>(accountDto);

            var result = await _userManager.CreateAsync(user, accountDto.Password);

            return result.Succeeded ? CreateSuccessfulResult() : CreateResultWithError(result);
        }

        private static OperationResult CreateResultWithError(IdentityResult result)
        {
            return new OperationResult { IsSuccessful = false, Errors = result.Errors.Select(error => error.Description) };
        }

        private static OperationResult CreateSuccessfulResult()
        {
            return new OperationResult { IsSuccessful = true };
        }
    }
}
