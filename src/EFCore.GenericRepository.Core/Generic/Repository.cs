using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace EFCore.GenericRepository.Core.Generic
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {

        /// <summary>
        /// The db context that is used in the repository.
        /// </summary>
        public DbContext DbContext { get; set; }

        private readonly RepositoryOptions<TEntity> _options;
        private readonly DbSet<TEntity> _set;

        public Repository(IOptions<RepositoryOptions<TEntity>> options, DbContext dbContext)
        {
            DbContext = dbContext;

            _options = options.Value;
            _set = DbContext.Set<TEntity>();
        }

        public async Task<TEntity[]> GetAllAsync()
        {
            return await _set.ToArrayAsync();
        }

    }
}
