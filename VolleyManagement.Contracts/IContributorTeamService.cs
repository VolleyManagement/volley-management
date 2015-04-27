namespace VolleyManagement.Contracts
{
    using System.Linq;
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
        IQueryable<ContributorTeam> Get();
    }
}
