namespace VolleyManagement.Data.Contracts
{
    /// <summary>
    /// The Repository interface.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Gets unit of work for data store.
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
    }
}