namespace EFCore.GenericRepository.Core
{
    /// <summary>
    /// Repository
    /// </summary>
    /// <typeparam name="TEntity">The entity for the Repository.</typeparam>
    public interface IRepository<TEntity>
        where TEntity : class
    {
    }
}
