namespace VolleyManagement.Data.MsSql.Services
{
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;

    using VolleyManagement.Data.Contracts;

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
            this._context = ((IObjectContextAdapter)new VolleyManagementContext()).ObjectContext;
            this._context.ContextOptions.LazyLoadingEnabled = true;
        }

        /// <summary>
        /// Gets context of the data source.
        /// </summary>
        public ObjectContext Context
        {
            get { return this._context; }
        }

        /// <summary>
        /// Commits all the changes.
        /// </summary>
        public void Commit()
        {
            this._context.SaveChanges();
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
