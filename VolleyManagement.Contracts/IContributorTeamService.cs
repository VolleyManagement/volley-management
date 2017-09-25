namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.ContributorsAggregate;

    /// <summary>
    /// Interface for ContributorService.
    /// </summary>
    public interface IContributorTeamService
    {
        /// <summary>
        /// Gets list of all contributors team.
        /// </summary>
        /// <returns>Return list of all contributors.</returns>
        Task<List<ContributorTeam>> Get();
    }
}
