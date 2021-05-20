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

        public async Task<TEntity> FindFirstAsync(Expression<Func<TEntity, bool>> predicate)
            => await _set.FirstOrDefaultAsync(predicate);

        public TEntity FindFirst(Expression<Func<TEntity, bool>> predicate)
            => _set.FirstOrDefault(predicate);

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
            => _set.Where(predicate);

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

                if(SaveChanges)
                {
                    SaveChanges = false;
                }

                return result;
            }

            return 0;
        }

    }
}
