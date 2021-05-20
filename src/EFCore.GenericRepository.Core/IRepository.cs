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
        public TEntity FindFirst(Expression<Func<TEntity, bool>> predicate);
        public Task<TEntity> FindFirstAsync(Expression<Func<TEntity, bool>> predicate);
        public Task<EntityEntry<TEntity>> AddAsync(TEntity entity);
        public void Update(TEntity entity, Action<TEntity> action);
        public Task UpdateAsync(Expression<Func<TEntity, bool>> predicate, Action<TEntity> action);
        public EntityEntry<TEntity> Remove(TEntity entity);
        public void RemoveRange(params TEntity[] entities);

        public ValueTask<int> SaveChangesAsync(bool force = false);

    }
}
