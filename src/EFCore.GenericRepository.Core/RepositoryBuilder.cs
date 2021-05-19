using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.GenericRepository.Core
{
    /// <summary>
    /// Builder for the options for <see cref="IRepository{TEntity}"/>
    /// </summary>
    /// <typeparam name="TEntity">The Entity for the <see cref="IRepository{TEntity}"/> where the options are for.</typeparam>
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
        /// If there were any changes to the <see cref="DbContext"/>, it will call SaveChangesAsync() on the <see cref="DbContext"/> when the Repository is disposed.
        /// </summary>
        /// <param name="val">Enable save changes at dispose</param>
        /// <returns>The same Repository Builder.</returns>
        public RepositoryBuilder<TEntity> WithSaveChangesAtDispose(bool val)
        {
            _options.SaveChangesOnDispose = val;

            return this;
        }

        /// <summary>
        /// Applies the options for the repository.
        /// </summary>
        /// <returns>The <see cref="IServiceCollection"/> so that multiple calls can be chained.</returns>
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
