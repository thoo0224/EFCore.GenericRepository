using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;

namespace EFCore.GenericRepository.Core.Generic
{
    public class RepositoryFactory<TEntity, TRepo> : IRepositoryFactory<TEntity, TRepo>
        where TEntity : class
        where TRepo : class, IRepository<TEntity>
    {

        private readonly GenericRepositoryOptions _options;
        private readonly IServiceProvider _serviceProvider;

        public RepositoryFactory(GenericRepositoryOptions options, IServiceProvider serviceProvider)
        {
            _options = options;
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc cref="IRepositoryFactory{TEntity,TRepo}.Create"/>
        public TRepo Create(DbContext existingDbContext = null)
        {
            var dbContextFactoryType = _options.DbContextFactoryType;
            var dbContextFactory = _serviceProvider.GetService(dbContextFactoryType);
            if(dbContextFactory == null)
            {
                throw new Exception("Failed to get the DbContextFactory from the service provider.");
            }

            var createDbContextMethod = dbContextFactoryType.GetMethod("CreateDbContext");
            var dbContext = existingDbContext ?? createDbContextMethod?.Invoke(dbContextFactory, new object[] { });
            if(dbContext == null)
            {
                throw new Exception("Failed to create DbContext.");
            }

            var repoType = typeof(TRepo);
            var repoConstructor = repoType.GetConstructors().FirstOrDefault();
            if(repoConstructor == null)
            {
                throw new Exception("Couldn't find a constructor in the repository.");
            }

            var arguments = new List<object>();
            foreach(var parameterInfo in repoConstructor.GetParameters())
            {
                var parameterType = parameterInfo.ParameterType;
                if(parameterType == typeof(DbContext))
                {
                    arguments.Add(dbContext);
                    continue;
                }

                var service = _serviceProvider.GetService(parameterType);
                if(service == null)
                {
                    throw new Exception($"Failed to get service from type {parameterType}.");
                }

                arguments.Add(service);
            }
            var repo = Activator.CreateInstance(repoType, arguments.ToArray());

            return repo as TRepo;
        }
    }
}
