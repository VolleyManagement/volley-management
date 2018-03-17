﻿namespace VolleyManagement.Data.MsSql
{
    using System.Data.Entity.Infrastructure;
    using System.Threading.Tasks;
    using Context;
    using Contracts;
    using System;

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
            get
            {
                return _context;
            }
        }

        /// <summary>
        /// Commits all the changes.
        /// </summary>
        public void Commit()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// Asynchronously commits all changes into the store
        /// </summary>
        /// <returns>Task to await</returns>
        public Task CommitAsync()
        {
            return _context.SaveChangesAsync();
        }

        /// <summary>
        /// IDisposable.Dispose method implementation.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// IDisposable.Dispose method implementation.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
