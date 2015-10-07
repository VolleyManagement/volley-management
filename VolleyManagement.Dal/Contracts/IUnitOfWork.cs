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
        /// Gets database context
        /// </summary>
        ObjectContext Context { get; }

        /// <summary>
        /// Commits all the changes the store.
        /// </summary>
        void Commit();

        /// <summary>
        /// Begins transaction
        /// </summary>
        /// <param name="isolationLevel">Level of transaction isolation</param>
        /// <returns>Manager of transaction</returns>
        IDbTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel);
    }
}
