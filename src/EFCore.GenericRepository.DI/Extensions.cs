using EFCore.GenericRepository.Core;
using EFCore.GenericRepository.Core.Generic;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using System;

namespace EFCore.GenericRepository.DI
{
    public static class Extensions
    {

        /// <summary>
        /// Configures the options for all the repositories.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services.</param>
        /// <param name="optionAction">Configures the options for the repositories.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection ConfigureRepositories(
            this IServiceCollection services,
            Action<GenericRepositoryOptions> optionAction = null)
        {
            var options = new GenericRepositoryOptions();
            optionAction?.Invoke(options);
            services.TryAddSingleton(options);
            services.TryAddSingleton(services);
            return services;
        }

        /// <summary>
        /// Adds the repository factory to the service collection.
        /// </summary>
        /// <typeparam name="TEntity">Entity type of the repository.</typeparam>
        /// <typeparam name="TFactory">Factory type for the repository factory. Default is <see cref="RepositoryFactory{TEntity,TRepo}"></see>/></typeparam>
        /// <typeparam name="TRepo"></typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services.</param>
        /// <param name="optionsAction">Action for the repository options.</param>
        /// <returns><see cref="RepositoryBuilder{TEntity}"/> for configuring options for the repository if they are not provided already.</returns>
        public static RepositoryBuilder<TEntity> AddRepository<TEntity, TFactory, TRepo>(
            this IServiceCollection services,
            Action<RepositoryOptions<TEntity>> optionsAction = null)
            where TEntity : class
            where TFactory : class, IRepositoryFactory<TEntity, TRepo>
            where TRepo : IRepository<TEntity>
        {
            var repoOptions = new RepositoryOptions<TEntity>();
            optionsAction?.Invoke(repoOptions);

            services.TryAddSingleton<IRepositoryFactory<TEntity, TRepo>, TFactory>();
            services.Configure<RepositoryOptions<TEntity>>(options =>
            {
                options.SaveChangesOnDispose = true;
            });

            return new RepositoryBuilder<TEntity>(services);
        }

        /// <inheritdoc cref="AddRepository{TEntity,TFactory,TRepo}"/>
        public static RepositoryBuilder<TEntity> AddRepository<TEntity, TRepo>(
            this IServiceCollection services,
            Action<RepositoryOptions<TEntity>> optionsAction = null)
            where TEntity : class
            where TRepo : class, IRepository<TEntity>
            => services.AddRepository<TEntity, RepositoryFactory<TEntity, TRepo>, TRepo>(optionsAction);

        /// <inheritdoc cref="AddRepository{TEntity,TFactory,TRepo}"/>
        public static RepositoryBuilder<TEntity> AddRepository<TEntity>(
            this IServiceCollection services,
            Action<RepositoryOptions<TEntity>> optionsAction = null)
            where TEntity : class
            => services.AddRepository<TEntity, RepositoryFactory<TEntity, Repository<TEntity>>, Repository<TEntity>>(optionsAction);

    }
}
