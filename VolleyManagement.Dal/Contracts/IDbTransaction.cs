namespace VolleyManagement.Dal.Contracts
{
    using System;
    using System.Data.Entity.Core.Objects;

    /// <summary>
    /// Describe methods to work with the store.
    /// </summary>
    public interface IDbTransaction : IDisposable
    {
        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rolls back a transaction
        /// </summary>
        void Rollback();

    }
}
