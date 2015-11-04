namespace VolleyManagement.Domain.ContributorsAggregate
{
    using System.Linq;

    using VolleyManagement.Data.Contracts;

    /// <summary>
    /// Defines specific contract for ContributorRepository
    /// </summary>
    public interface IContributorTeamRepository : IGenericRepository<ContributorTeam>
    {
        /// <summary>
        /// Gets all contributors team.
        /// </summary>
        /// <returns>Collection of contributors from the repository.</returns>
        IQueryable<ContributorTeam> Find();
    }
}
