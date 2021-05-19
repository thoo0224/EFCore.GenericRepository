using Microsoft.EntityFrameworkCore;

namespace EFCore.GenericRepository.Core
{
    /// <summary>
    /// Factory for the Repository.
    /// </summary>
    /// <typeparam name="TEntity">Entity of the Repository</typeparam>
    /// <typeparam name="TRepo">Repository</typeparam>
    public interface IRepositoryFactory<TEntity, out TRepo>
        where TEntity : class
        where TRepo : IRepository<TEntity>
    {

        /// <summary>
        /// Creates the repository
        /// </summary>
        /// <param name="dbContext">If there's already a db context, we can use that for the new repository.</param>
        /// <returns>Repository</returns>
        public TRepo Create(DbContext dbContext = null);

    }
}
