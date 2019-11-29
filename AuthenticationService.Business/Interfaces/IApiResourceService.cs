using AuthenticationService.Domain.Common;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationService.Business.Interfaces
{
    public interface IApiResourceService
    {
        Task<OperationResult<ApiResource>> AddAsync(ApiResource apiResource);

        Task<IEnumerable<ApiResource>> GetAsync(int page = 1, int pageSize = 10);

        Task<ApiResource> GetByIdAsync(int id);

        Task<OperationResult> DeleteAsync(int id);
    }
}
