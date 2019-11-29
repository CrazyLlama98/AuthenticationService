using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationService.Business.Interfaces;
using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.Repositories;
using AuthenticationService.Domain.UnitOfWork;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;

namespace AuthenticationService.Business.Implementation
{
    public class ApiResourceService : IApiResourceService
    {
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IUnitOfWork<ConfigurationDbContext> _unitOfWork;

        public ApiResourceService(
            IApiResourceRepository apiResourceRepository,
            IUnitOfWork<ConfigurationDbContext> unitOfWork)
        {
            _apiResourceRepository = apiResourceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<ApiResource>> AddAsync(ApiResource apiResource)
        {
            var entity = apiResource.ToEntity();
            await _apiResourceRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return new OperationResult<ApiResource>
            {
                Data = entity.ToModel(), IsSuccessful = true
            };
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var entity = await _apiResourceRepository.GetByIdAsync(id);
            if (entity == null) { 
                return null; 
            }

            _apiResourceRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return new OperationResult { IsSuccessful = true };
        }

        public async Task<IEnumerable<ApiResource>> GetAsync(int page = 1, int pageSize = 10)
        {
            var entities = await _apiResourceRepository.Filter()
                .OrderBy(apiResource => apiResource.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return entities.Select(apiResource => apiResource.ToModel());
        }

        public async Task<ApiResource> GetByIdAsync(int id)
        {
            var entity = await _apiResourceRepository.GetByIdAsync(id);

            return entity?.ToModel();
        }
    }
}
