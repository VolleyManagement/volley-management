namespace VolleyManagement.Data.MsSql.Infrastructure
{
    using System;

    using VolleyManagement.Dal.Contracts;

    /// <summary>
    /// Defines implementation of the IDbTransaction contract.
    /// </summary>
    public class DbTransactionAdapter : IDbTransaction
    {
        private System.Data.IDbTransaction _transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTransactionAdapter"/> class.
        /// </summary>
        /// <param name="transaction">Transaction manager</param>
        public DbTransactionAdapter(System.Data.IDbTransaction transaction)
        {
            if (transaction == null)
            {
                throw new NullReferenceException();
            }

            this._transaction = transaction;
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        public void Commit()
        {
            this._transaction.Commit();
        }

        /// <summary>
        /// Rolls back a transaction
        /// </summary>
        public void Rollback()
        {
            this._transaction.Rollback();
        }

        /// <summary>
        /// Disposes object
        /// </summary>
        public void Dispose()
        {
            this._transaction.Dispose();
        }
    }
}
