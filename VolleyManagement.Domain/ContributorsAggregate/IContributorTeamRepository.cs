namespace VolleyManagement.Domain.ContributorsAggregate
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Contracts;

    /// <summary>
    /// Defines specific contract for ContributorRepository
    /// </summary>
    public interface IContributorTeamRepository : IGenericRepository<ContributorTeam>
    {
        /// <summary>
        /// Gets all contributors team.
        /// </summary>
        /// <returns>Collection of contributors from the repository.</returns>
        Task<List<ContributorTeam>> Find();
    }
}
