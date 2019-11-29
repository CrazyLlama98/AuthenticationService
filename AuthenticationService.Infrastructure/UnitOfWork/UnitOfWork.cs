using System;
using System.Threading.Tasks;
using AuthenticationService.Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Infrastructure.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>
        where TContext : DbContext
    {
        protected TContext DbContext { get; }
        protected IServiceProvider ServiceProvider { get; }

        public UnitOfWork(TContext dbContext, IServiceProvider serviceProvider)
        {
            DbContext = dbContext;
            ServiceProvider = serviceProvider;
        }

        public TRepository GetRepository<TRepository>() where TRepository : class
        {
            return ServiceProvider.GetService(typeof(TRepository)) as TRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }
    }
}
