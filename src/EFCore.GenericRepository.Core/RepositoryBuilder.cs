using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace EFCore.GenericRepository.Core
{
    /// <summary>
    /// Builder for the options for <see cref="IRepository{TEntity}"/>
    /// </summary>
    /// <typeparam name="TEntity">The Entity for the <see cref="IRepository{TEntity}"/> where the options are for.</typeparam>
    public class RepositoryBuilder<TEntity> : IDisposable
        where TEntity : class
    {

        private readonly RepositoryOptions<TEntity> _options;
        private readonly IServiceCollection _services;

        private bool _applied;

        public RepositoryBuilder(IServiceCollection services)
        {
            _options = new RepositoryOptions<TEntity>();
            _services = services;
        }

        /// <summary>
        /// Calls <see cref="Dispose"/> when the builder gets deconstructed so it will configure the repository options if <see cref="Apply"/> was not called.
        /// </summary>
        ~RepositoryBuilder()
        {
            Dispose();
        }

        /// <summary>
        /// If there were any changes to the <see cref="DbContext"/>, it will call SaveChangesAsync() on the <see cref="DbContext"/> when the Repository is disposed.
        /// </summary>
        /// <param name="val">Enable save changes at dispose</param>
        /// <returns>The same Repository Builder.</returns>
        public RepositoryBuilder<TEntity> WithSaveChangesOnDispose(bool val)
        {
            _options.SaveChangesOnDispose = val;

            return this;
        }

        /// <summary>
        /// Applies the options for the repository.
        /// </summary>
        /// <returns>The used <see cref="IServiceCollection"/> for adding the repository, so multiple calls can be chained.</returns>
        public IServiceCollection Apply()
        {
            Configure();

            return _services;
        }

        /// <summary>
        /// Makes sure the repository options are configured even when <see cref="Apply"/> was not called.
        /// </summary>
        public void Dispose()
        {
            Configure();
        }

        private void Configure()
        {
            if (_applied)
            {
                return;
            }

            _services.Configure<RepositoryOptions<TEntity>>(options =>
            {
                options.SaveChangesOnDispose = _options.SaveChangesOnDispose;
            });

            _applied = true;
        }

    }
}
