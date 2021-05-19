using Microsoft.EntityFrameworkCore;

namespace EFCore.GenericRepository.Core
{
    /// <summary>
    /// Factory for the Repository. This will create the repository using services from the service collection.
    /// </summary>
    /// <typeparam name="TEntity">Entity of the <see cref="IRepository{TEntity}"/></typeparam>
    /// <typeparam name="TRepo">Type for <see cref="IRepository{TEntity}"/>\</typeparam>
    public interface IRepositoryFactory<TEntity, out TRepo>
        where TEntity : class
        where TRepo : IRepository<TEntity>
    {

        /// <summary>
        /// Creates the repository
        /// </summary>
        /// <param name="existingDbContext">An existing <see cref="DbContext"/> so we don't have to create a new one.</param>
        /// <returns>The <see cref="IRepository{TEntity}"/> that has been created.</returns>
        public TRepo Create(DbContext existingDbContext = null);

    }
}
