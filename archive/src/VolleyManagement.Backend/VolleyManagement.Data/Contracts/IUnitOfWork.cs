namespace VolleyManagement.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Describe methods to work with the store.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commits all the changes the store.
        /// </summary>
        void Commit();

        /// <summary>
        /// Asynchronously commits all changes into the store
        /// </summary>
        /// <returns>Task to await</returns>
        Task CommitAsync();
    }
}
