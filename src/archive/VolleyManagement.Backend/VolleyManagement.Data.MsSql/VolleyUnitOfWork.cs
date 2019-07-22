using VolleyManagement.Crosscutting.Contracts.Providers;

namespace VolleyManagement.Data.MsSql
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
        /// Initializes a new instance of the <see cref="VolleyUnitOfWork"/> class.
        /// </summary>
        public VolleyUnitOfWork(IConfigurationProvider configurationProvider)
        {
            Context = new VolleyManagementEntities(configurationProvider.GetVolleyManagementEntitiesConnectionString());
            ((IObjectContextAdapter)Context).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
        }

        /// <summary>
        /// Gets context of the data source.
        /// </summary>
        internal VolleyManagementEntities Context { get; }

        /// <summary>
        /// Commits all the changes.
        /// </summary>
        public void Commit()
        {
            Context.SaveChanges();
        }

        /// <summary>
        /// Asynchronously commits all changes into the store
        /// </summary>
        /// <returns>Task to await</returns>
        public Task CommitAsync()
        {
            return Context.SaveChangesAsync();
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
                Context.Dispose();
            }
        }
    }
}
