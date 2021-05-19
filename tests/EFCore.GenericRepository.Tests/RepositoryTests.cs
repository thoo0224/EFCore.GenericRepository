using System.Diagnostics;
using System.Threading.Tasks;
using EFCore.GenericRepository.Core.Generic;

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
            var repo = _factory.Create();

            var result = await repo.GetAllAsync();
            Debugger.Break();
        }
    }
}
