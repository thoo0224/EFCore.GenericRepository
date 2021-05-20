<div align="center">

# EFCore.GenericRepository

<img src="https://github.com/thoo0224/EFCore.GenericRepository/blob/main/resources/icon.png" width="100"/>

[![Nuget](https://img.shields.io/nuget/v/EFCore.GenericRepositories?logo=nuget)](https://www.nuget.org/packages/GenericRepositories) [![Nuget DLs](https://img.shields.io/nuget/dt/EFCore.GenericRepositories?logo=nuget)](https://www.nuget.org/packages/EFCore.GenericRepositories) [![GitHub License](https://img.shields.io/github/license/thoo0224/EFCore.GenericRepositories)](https://github.com/thoo0224/EFCore.GenericRepository/blob/master/LICENSE)

</div>

### NuGet
```
Install-Package EFCore.GenericRepositories
```

### How to start?
```cs
public void ConfigureServices(IServiceCollection services) 
{
	collection.ConfigureRepositories();
	collection.AddRepository<Entity>()
		// Optional for if you don't want to save changes when the repository gets disposed.
		.WithSaveChangesOnDispose(false);

	// You can also create your own implementation of the Repository Factory the Repository itself and registere it like this,
	collection.AddRepository<Entity, RepositoryImplementation>();
	// or
	collection.AddRepository<Entity, FactoryImplementation, RepositoryImplementation>();
}

private readonly IRepositoryFactory<Entity, Repository<Entity>> _factory;

// Inject the repository factory into a class using Microsoft.Extensions.DependencyInjection
public Class(IRepositoryFactory<Entity, Repository<Entity>> factory)
{
	_factory = factory;
}

public async Task DoSomething() 
{
	// Create a repository using the factory, will construct it with dependencies from the service provider where the repository was added to.
	// Recommended to use `await using` so it will dispose the repository automatically when it's not used anymore.
	await using var repo = _factory.Create(); // You can pass an exisiting DbContext to the Create method so it doesn't need to make another one.

	// When you add something, it will automatically save changes when the repository is disposed.
	await repo.AddAsync(entity);
	
	// The rest is pretty self explanatory.
}
```

### Contribution
Any type of contribution is appreciated!

### License
EFCore.GenericRepository (Apache-2.0) [License](https://github.com/thoo0224/EFCore.GenericRepositories/blob/main/LICENSE)