using EFCore.GenericRepository.Core.Generic;
using EFCore.GenericRepository.DI;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.GenericRepository.Tests
{
    public class Startup
    {

        public class AppDbContext : DbContext
        {

            public DbSet<Test> Tests { get; set; }

            public AppDbContext(DbContextOptions options)
                : base(options) { }

        }

        public class Test
        {

        }

        public void ConfigureServices(IServiceCollection collection)
        {
            collection.AddDbContextFactory<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("GenericRepository");
            });
            collection.ConfigureRepositories(options =>
            {
                options.DbContextFactoryType = typeof(IDbContextFactory<AppDbContext>);
            });
            collection.AddRepository<Test, RepositoryFactory<Test, Repository<Test>>, Repository<Test>>();
        }

    }
}
