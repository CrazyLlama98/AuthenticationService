using AuthenticationService.Domain.Dtos;
using AuthenticationService.Domain.Common;
using System.Threading.Tasks;

namespace AuthenticationService.Business.Interfaces
{
    public interface IAccountService
    {
        Task<OperationResult> CreateAccountAsync(AccountDto accountDto);
    }
}
