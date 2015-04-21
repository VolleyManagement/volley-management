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

        /// <summary>
        /// Create new contributor.
        /// </summary>
        /// <param name="contributor">New contributor.</param>
        void Create(Contributor contributor);

        /// <summary>
        /// Edit contributor profile.
        /// </summary>
        /// <param name="contributor">Updated contributor data.</param>
        void Edit(Contributor contributor);

        /// <summary>
        /// Find contributor by id.
        /// </summary>
        /// <param name="id">contributor id.</param>
        /// <returns>Found contributor.</returns>
        Contributor Get(int id);

        /// <summary>
        /// Delete contributor by id.
        /// </summary>
        /// <param name="id">contributor id.</param>
        void Delete(int id);
    }
}
