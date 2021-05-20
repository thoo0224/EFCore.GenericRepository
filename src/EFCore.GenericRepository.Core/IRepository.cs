using Microsoft.EntityFrameworkCore.ChangeTracking;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFCore.GenericRepository.Core
{
    /// <summary>
    /// Repository
    /// </summary>
    /// <typeparam name="TEntity">The entity for the Repository.</typeparam>
    public interface IRepository<TEntity> : IAsyncDisposable
        where TEntity : class
    {

        public IQueryable<TEntity> GetAll();
        public IQueryable<TEntity> GetAllNoTracking();

        public Task<EntityEntry<TEntity>> AddAsync(TEntity entity);

        public ValueTask<int> SaveChangesAsync(bool force = false);

    }
}
