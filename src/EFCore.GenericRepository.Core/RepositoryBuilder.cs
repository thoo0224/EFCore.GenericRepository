using Microsoft.Extensions.DependencyInjection;

namespace EFCore.GenericRepository.Core
{
    /// <summary>
    /// Builder for the options for repositories
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class RepositoryBuilder<TEntity>
        where TEntity : class
    {

        private readonly RepositoryOptions<TEntity> _options;
        private readonly IServiceCollection _services;

        public RepositoryBuilder(IServiceCollection services)
        {
            _options = new RepositoryOptions<TEntity>();
            _services = services;
        }

        /// <summary>
        /// If there were any changes to the DbContext, it will call SaveChangesAsync() on the DbContext when the Repository is disposed.
        /// </summary>
        /// <param name="val">Enable save changes at dispose</param>
        /// <returns>Repository Builder</returns>
        public RepositoryBuilder<TEntity> WithSaveChangesAtDispose(bool val)
        {
            _options.SaveChangesOnDispose = val;

            return this;
        }

        /// <summary>
        /// Applies the options for the repository
        /// </summary>
        /// <returns>Service collection</returns>
        public IServiceCollection Apply()
        {
            _services.Configure<RepositoryOptions<TEntity>>(options =>
            {
                options.SaveChangesOnDispose = _options.SaveChangesOnDispose;
            });

            return _services;
        }

    }
}
