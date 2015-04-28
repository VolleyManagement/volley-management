namespace VolleyManagement.Dal.MsSql.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VolleyManagement.Dal.Contracts;

    /// <summary>
    /// Defines implementation of the IDbTransaction contract.
    /// </summary>
    public class VolleyDbTransaction : IDbTransaction
    {
        private System.Data.IDbTransaction _transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolleyDbTransaction"/> class.
        /// </summary>
        /// <param name="transaction">Transaction manager</param>
        public VolleyDbTransaction(System.Data.IDbTransaction transaction)
        {
            this._transaction = transaction;
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        public void Commit()
        {
            _transaction.Commit();
        }

        /// <summary>
        /// Rolls back a transaction
        /// </summary>
        public void Rollback()
        {
            _transaction.Rollback();
        }

        /// <summary>
        /// Disposes object
        /// </summary>
        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
}
