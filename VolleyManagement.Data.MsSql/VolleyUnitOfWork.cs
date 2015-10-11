namespace VolleyManagement.Data.MsSql
{
    using System.Data.Entity.Core;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;

    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.MsSql.Context;

    /// <summary>
    /// Defines Entity Framework implementation of the IUnitOfWork contract.
    /// </summary>
    internal class VolleyUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Context of the data source.
        /// </summary>
        private readonly VolleyManagementEntities _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolleyUnitOfWork"/> class.
        /// </summary>
        public VolleyUnitOfWork()
        {
            _context = new VolleyManagementEntities();
            ((IObjectContextAdapter)_context).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
        }

        /// <summary>
        /// Gets context of the data source.
        /// </summary>
        internal VolleyManagementEntities Context
        {
            get { return this._context; }
        }

        /// <summary>
        /// Commits all the changes.
        /// </summary>
        public void Commit()
        {
            try
            {
                this._context.SaveChanges();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new InvalidKeyValueException("Entity with request Id does not exist", ex);
            }
        }

        /// <summary>
        /// IDisposable.Dispose method implementation.
        /// </summary>
        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}
