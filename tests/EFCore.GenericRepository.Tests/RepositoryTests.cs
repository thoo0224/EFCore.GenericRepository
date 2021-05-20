using EFCore.GenericRepository.Core;
using EFCore.GenericRepository.Core.Generic;

using System.Linq;
using System.Threading.Tasks;

using Xunit;

namespace EFCore.GenericRepository.Tests
{
    public class RepositoryTests
    {

        private readonly IRepositoryFactory<Startup.Test, Repository<Startup.Test>> _factory;

        public RepositoryTests(IRepositoryFactory<Startup.Test, Repository<Startup.Test>> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task FindTest()
        {
            await AddAsync();

            await using var repo = _factory.Create();
            var one = await repo.FindFirstAsync(x => x.Id.Equals("test"));
            var two = await repo.FindFirstAsync(x => x.Id.Equals("test2"));

            Assert.NotNull(one);
            Assert.Null(two);
        }

        [Fact]
        public async Task AddTest()
        {
            await using var repo = _factory.Create();

            await AddAsync();

            var actual = repo.GetAllNoTracking().Count();
            var expected = 1;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task UpdateTest()
        {
            await AddAsync();

            await using var repo = _factory.Create();
            var one = await repo.FindFirstAsync(x => x.Id.Equals("test"));

            Assert.NotNull(one);

            repo.Update(one, x => x.Name = "thoo2");

            var expected = "thoo2";
            var actual = one.Name;

            Assert.Equal(expected, actual);
        }

        private async Task AddAsync()
        {
            await using var repo = _factory.Create();
            if (await repo.FindFirstAsync(x => x.Id.Equals("test")) != null)
            {
                return;
            }

            await repo.AddAsync(new Startup.Test
            {
                Id = "test",
                Name = "thoo"
            });
        }

    }
}
