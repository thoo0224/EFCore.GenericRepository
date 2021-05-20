using EFCore.GenericRepository.Core.Generic;

using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace EFCore.GenericRepository.Tests
{
    public class RepositoryTests
    {

        private readonly RepositoryFactory<Startup.Test, Repository<Startup.Test>> _factory;
        private readonly ITestOutputHelper _output;

        public RepositoryTests(RepositoryFactory<Startup.Test, Repository<Startup.Test>> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        [Fact]
        public async Task Test()
        {
            {
                await using var repo = _factory.Create();
                await repo.AddAsync(new Startup.Test 
                {
                    Id = "test"
                });
            }

            await using var repo2 = _factory.Create();

            var actual = repo2.GetAllNoTracking().Count();
            var expected = 1;

            Assert.Equal(expected, actual);
        }

    }
}
