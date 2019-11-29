using FluentApiGenericRepository.Interfaces.Repository;
using IdentityServer4.EntityFramework.Entities;

namespace AuthenticationService.Domain.Repositories
{
    public interface IApiResourceRepository : IGenericRepository<ApiResource>
    {
    }
}
