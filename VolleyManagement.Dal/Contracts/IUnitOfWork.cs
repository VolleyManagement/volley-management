namespace VolleyManagement.Dal.Contracts
{
    using System;

    using VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Describe methods to work with the store.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the Tournament repository.
        /// </summary>
        IRepository<Tournament> Tournaments { get; }

        /// <summary>
        /// Commits all the changes the store.
        /// </summary>
        void Commit();
    }
}
