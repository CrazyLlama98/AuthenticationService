using FluentApiGenericRepository.Interfaces.Repository;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.UnitOfWork
{
    public interface IUnitOfWork<T> 
        where T : class
    {
        TRepository GetRepository<TRepository>()
            where TRepository : class;

        Task<int> SaveChangesAsync();
    }
}
