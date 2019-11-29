using AuthenticationService.Domain.Repositories;
using FluentApiGenericRepository.Implementation.Repository;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;

namespace AuthenticationService.Infrastructure.Repositories
{
    public class ApiResourceRepository : GenericRepository<ApiResource>, IApiResourceRepository
    {
        public ApiResourceRepository(ConfigurationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
