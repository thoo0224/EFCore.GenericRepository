using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace EFCore.GenericRepository.Core
{
    public class GenericRepositoryOptions
    {

        /// <summary>
        /// Optional, if null it will search through the <see cref="IServiceCollection"/> for a <see cref="DbContextFactory{TContext}"/>
        /// </summary>
        public Type DbContextFactoryType { get; set; }

    }
}
