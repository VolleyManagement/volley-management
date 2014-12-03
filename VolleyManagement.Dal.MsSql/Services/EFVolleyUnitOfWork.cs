namespace VolleyManagement.Dal.MsSql.Services
{
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;

    using VolleyManagement.Dal.Contracts;

    /// <summary>
    /// Defines Entity Framework implementation of the IUnitOfWork contract.
    /// </summary>
    internal class EFVolleyUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Context of the data source.
        /// </summary>
        private readonly ObjectContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EFVolleyUnitOfWork"/> class.
        /// </summary>
        public EFVolleyUnitOfWork()
        {
            context = ((IObjectContextAdapter)new VolleyManagementContext()).ObjectContext;
            context.ContextOptions.LazyLoadingEnabled = true;
        }

        /// <summary>
        /// Gets context of the data source.
        /// </summary>
        public ObjectContext Context
        {
            get { return context; }
        }

        /// <summary>
        /// Commits all the changes.
        /// </summary>
        public void Commit()
        {
            context.SaveChanges();
        }

        /// <summary>
        /// IDisposable.Dispose method implementation.
        /// </summary>
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
