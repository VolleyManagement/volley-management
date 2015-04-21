namespace VolleyManagement.Dal.Contracts
{
    using System;
    using System.Data.Entity.Core.Objects;
    using System.Transactions;

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
        /// Begin transaction with database
        /// </summary>
        TransactionScope BeginTransaction();

        /// <summary>
        /// Commit changes in current transaction
        /// </summary>
        void CommitTransaction();
    }
}
