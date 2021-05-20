using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFCore.GenericRepository.Core.Generic
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {

        /// <summary>
        /// The db context that is used in the repository.
        /// </summary>
        public DbContext DbContext { get; set; }

        /// <summary>
        /// Used when the repository is disposed, it will save the changes that have been made.
        /// </summary>
        public bool SaveChanges { get; set; }

        private readonly RepositoryOptions<TEntity> _options;
        private readonly DbSet<TEntity> _set;

        public Repository(IOptions<RepositoryOptions<TEntity>> options, DbContext dbContext)
        {
            DbContext = dbContext;

            _options = options.Value;
            _set = DbContext.Set<TEntity>();
        }

        /// <summary>
        /// Gets all the entities from the <see cref="DbSet{TEntity}"/>.
        /// </summary>
        /// <returns>Queryable</returns>
        public IQueryable<TEntity> GetAll()
        {
            return _set.AsQueryable();
        }

        /// <summary>
        /// Gets all the entities from the <see cref="DbSet{TEntity}"/> with tracking.
        /// </summary>
        /// <returns>Queryable</returns>
        public IQueryable<TEntity> GetAllNoTracking()
            => _set.AsQueryable()
                .AsNoTracking();

        public async Task<EntityEntry<TEntity>> AddAsync(TEntity entity)
        {
            SaveChanges = true;
            return await _set.AddAsync(entity);
        }

        /// <summary>
        /// Finds the first entity that succeeds the predicate.
        /// </summary>
        /// <param name="predicate">The predicate</param>
        /// <returns>Entity or null if it doesn't exist.</returns>
        public async Task<TEntity> FindFirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _set.FirstOrDefaultAsync(predicate);
        }

        /// <inheritdoc cref="FindFirstAsync"/>
        public TEntity FindFirst(Expression<Func<TEntity, bool>> predicate)
        {
            return _set.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Find multiple entities that succeeds the predicate.
        /// </summary>
        /// <param name="predicate">The predicate</param>
        /// <returns>An <see cref="IQueryable{T}"/> of the entities.</returns>
        public IQueryable<TEntity> FindMultiple(Expression<Func<TEntity, bool>> predicate)
        {
            return _set.Where(predicate);
        }

        /// <summary>
        /// Updated the <see cref="entity"/> with the <see cref="action"/> (Tracking must be enabled)
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="action">Action that updates the properties of <see cref="entity"/></param>
        public void Update(TEntity entity, Action<TEntity> action)
        {
            action.Invoke(entity);
            SaveChanges = true;
        }

        /// <summary>
        /// Updated the entity that has been found with the <see cref="action"/> (Tracking must be enabled)
        /// </summary>
        /// <param name="predicate">Predicate to find the entity.</param>
        /// <param name="action">Action that updates the properties of the entity.</param>
        public async Task UpdateAsync(
            Expression<Func<TEntity, bool>> predicate,
            Action<TEntity> action)
        {
            var entity = await FindFirstAsync(predicate);
            if (entity != null)
            {
                action.Invoke(entity);
            }

            Update(entity, action);
        }

        /// <summary>
        /// Removes the entity
        /// </summary>
        /// <param name="entity">The entity to remove</param>
        /// <returns></returns>
        public EntityEntry<TEntity> Remove(TEntity entity)
        {
            SaveChanges = true;
            return _set.Remove(entity);
        }

        /// <summary>
        /// Removed a rage of entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public void RemoveRange(params TEntity[] entities)
        {
            SaveChanges = true;
            _set.RemoveRange(entities);
        }

        /// <summary>
        /// Disposes the repository, if there were any changes it will save them.
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            await SaveChangesAsync();
        }

        /// <summary>
        /// Saves the changes to the DbContext
        /// </summary>
        /// <param name="force">If it needs to get forced, since it will not save the changes if <see cref="SaveChanges"/> is false.</param>
        /// <returns></returns>
        public async ValueTask<int> SaveChangesAsync(bool force = false)
        {
            if (force || SaveChanges && _options.SaveChangesOnDispose)
            {
                var result = await DbContext.SaveChangesAsync()
                    .ConfigureAwait(false);

                if (SaveChanges)
                {
                    SaveChanges = false;
                }

                return result;
            }

            return 0;
        }

    }
}