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
        /// <param name="services">Service collection</param>
        /// <param name="optionAction">Option action</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection ConfigureRepositories(
            this IServiceCollection services,
            Action<GenericRepositoryOptions> optionAction)
        {
            var options = new GenericRepositoryOptions();
            optionAction?.Invoke(options);
            services.AddSingleton(options);
            return services;
        }

        /// <summary>
        /// Adds the repository factory to the service collection.
        /// </summary>
        /// <typeparam name="TEntity">Entity type of the repository</typeparam>
        /// <typeparam name="TFactory">Factory type for the repository factory. Default is <see cref="RepositoryFactory{TEntity,TRepo}"></see>/></typeparam>
        /// <typeparam name="TRepo"></typeparam>
        /// <param name="services">Service collection</param>
        /// <param name="optionsAction">Action for the repository options.</param>
        /// <returns>Repository builder</returns>
        public static RepositoryBuilder<TEntity> AddRepository<TEntity, TFactory, TRepo>(
            this IServiceCollection services,
            Action<RepositoryOptions<TEntity>> optionsAction = null)
            where TEntity : class
            where TFactory : class, IRepositoryFactory<TEntity, TRepo>
            where TRepo : IRepository<TEntity>
        {
            var options = new RepositoryOptions<TEntity>();
            optionsAction?.Invoke(options);

            services.TryAddSingleton<TFactory>();
            services.Configure<RepositoryOptions<TEntity>>(options =>
            {
                options.SaveChangesOnDispose = true;
            });

            return new RepositoryBuilder<TEntity>(services);
        } 

    }
}
