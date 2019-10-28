using AuthenticationService.Dtos;
using System.Threading.Tasks;

namespace AuthenticationService.Services.Interfaces
{
    public interface IAccountService
    {
        Task<OperationResultDto> CreateAccountAsync(AccountDto accountDto);
    }
}
