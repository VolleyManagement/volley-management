namespace VolleyManagement.Dal.MsSql.Services
{
    using System.Data.Entity.Core;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.Exceptions;

    /// <summary>
    /// Defines Entity Framework implementation of the IUnitOfWork contract.
    /// </summary>
    internal class VolleyUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Context of the data source.
        /// </summary>
        private readonly ObjectContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolleyUnitOfWork"/> class.
        /// </summary>
        public VolleyUnitOfWork()
        {
            _context = ((IObjectContextAdapter)new VolleyManagementContext()).ObjectContext;
            _context.ContextOptions.LazyLoadingEnabled = true;
        }

        /// <summary>
        /// Gets context of the data source.
        /// </summary>
        public ObjectContext Context
        {
            get { return _context; }
        }

        /// <summary>
        /// Commits all the changes.
        /// </summary>
        public void Commit()
        {
            try
            {
                _context.SaveChanges();
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
            _context.Dispose();
        }

        /// <summary>
        /// Begins transaction
        /// </summary>
        /// <param name="isolationLevel">Level of transaction isolation</param>
        /// <returns>Current transaction manager</returns>
        public IDbTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            _context.Connection.Open();

            // TODO: Revisit connection opening approach

            var transaction = _context.Connection.BeginTransaction(isolationLevel);
            return new DbTransactionAdapter(transaction);
        }
    }
}
