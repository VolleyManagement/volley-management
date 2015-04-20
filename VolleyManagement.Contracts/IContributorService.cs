namespace VolleyManagement.Contracts
{
    using System.Linq;
    using Domain.Contributors;

    /// <summary>
    /// Interface for ContributorService.
    /// </summary>
    public interface IContributorService
    {
        /// <summary>
        /// Gets list of all contributors.
        /// </summary>
        /// <returns>Return list of all contributors.</returns>
        IQueryable<Contributor> Get();
    }
}
