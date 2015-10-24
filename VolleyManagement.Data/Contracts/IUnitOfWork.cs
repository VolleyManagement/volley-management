namespace VolleyManagement.Data.Contracts
{
    using System;
    using System.Data.Entity.Core.Objects;

    /// <summary>
    /// Describe methods to work with the store.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commits all the changes the store.
        /// </summary>
        void Commit();
    }
}
