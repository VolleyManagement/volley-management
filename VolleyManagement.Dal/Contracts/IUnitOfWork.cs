namespace VolleyManagement.Dal.Contracts
{
    using System;
    using System.Data.Objects;

    using VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Describe methods to work with the store.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets database context
        /// </summary>
        ObjectContext Context { get; }

        /// <summary>
        /// Commits all the changes the store.
        /// </summary>
        void Commit();
    }
}
