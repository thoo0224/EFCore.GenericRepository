namespace EFCore.GenericRepository.Core
{
    public class RepositoryOptions<TEntity>
        where TEntity : class
    {

        /// <summary>
        /// If enabled, it will save changes to the database if the repository is disposed.
        /// </summary>
        public bool SaveChangesOnDispose { get; set; }

    }
}
