using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace EFCore.GenericRepository.Core.Generic
{
    /// <summary>
    /// Created the repository with the needed dependencies from the <see cref="IServiceProvider"/>
    /// </summary>
    /// <typeparam name="TEntity">Entity Type for the repository</typeparam>
    /// <typeparam name="TRepo">Repository Type</typeparam>
    public class RepositoryFactory<TEntity, TRepo> : IRepositoryFactory<TEntity, TRepo>
        where TEntity : class
        where TRepo : class, IRepository<TEntity>
    {

        private readonly GenericRepositoryOptions _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceCollection _serviceCollection;

        private readonly Type _dbContextFactoryType;
        private readonly object _dbContextFactory;

        public RepositoryFactory(
            GenericRepositoryOptions options, 
            IServiceProvider serviceProvider, 
            IServiceCollection serviceCollection)
        {
            _options = options;
            _serviceProvider = serviceProvider;
            _serviceCollection = serviceCollection;

            _dbContextFactory = GetDbContextFactory();
            _dbContextFactoryType = _dbContextFactory?.GetType();
        }

        /// <inheritdoc cref="IRepositoryFactory{TEntity,TRepo}.Create"/>
        public TRepo Create(DbContext existingDbContext = null)
        {
            var repoType = typeof(TRepo);
            var repoConstructor = repoType.GetConstructors().FirstOrDefault();
            if(repoConstructor == null)
            {
                throw new Exception("Couldn't find a constructor in the repository.");
            }

            var dbContext = existingDbContext ?? CreateDbContext();
            if (dbContext == null)
            {
                throw new Exception("Failed to create DbContext.");
            }

            var arguments = GetRepositoryArguments(dbContext, repoConstructor);
            var repo = repoConstructor.Invoke(arguments.ToArray());

            return repo as TRepo;
        }

        private IEnumerable<object> GetRepositoryArguments(object dbContext, ConstructorInfo constructor)
        {
            var arguments = new List<object>();
            if (constructor == null)
            {
                throw new Exception("Couldn't find a constructor in the repository.");
            }

            foreach (var parameterInfo in constructor.GetParameters())
            {
                var parameterType = parameterInfo.ParameterType;
                if (parameterType == typeof(DbContext))
                {
                    arguments.Add(dbContext);
                    continue;
                }

                var service = _serviceProvider.GetService(parameterType);
                if (service == null)
                {
                    throw new Exception($"Failed to get service from type {parameterType}.");
                }

                arguments.Add(service);
            }

            return arguments;
        }

        private object GetDbContextFactory()
        {
            var dbContextFactoryType = _options.DbContextFactoryType ?? GetDbContextFactoryType();
            if(dbContextFactoryType == null)
            {
                throw new Exception("Failed to get the DbContextFactory type from the service collection, please configure the repositories with the DbContextFactory type.");
            }

            var dbContextFactory = _serviceProvider.GetService(dbContextFactoryType);
            if (dbContextFactory == null)
            {
                throw new Exception("Failed to get the DbContextFactory from the service provider.");
            }

            return dbContextFactory;
        }

        private Type GetDbContextFactoryType()
        {
            var service = _serviceCollection
                .FirstOrDefault(serviceDescriptor =>
                {
                    var type = serviceDescriptor.ServiceType;

                    var typeName = type.Name;
                    var dbContextFactoryTypeName = typeof(IDbContextFactory<>).Name;

                    return typeName.Equals(dbContextFactoryTypeName, StringComparison.CurrentCultureIgnoreCase);
                });

            return service?.ServiceType;
        }

        private DbContext CreateDbContext()
        {
            var createDbContextMethod = _dbContextFactoryType.GetMethod("CreateDbContext");
            var dbContext = createDbContextMethod?.Invoke(_dbContextFactory, new object[] { });

            return dbContext as DbContext;
        }

    }
}
