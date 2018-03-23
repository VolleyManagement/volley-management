namespace VolleyManagement.Domain.ContributorsAggregate
{
    using System.Collections.Generic;
    using Data.Contracts;

    /// <summary>
    /// Defines specific contract for ContributorRepository
    /// </summary>
    public interface IContributorTeamRepository : IGenericRepository<ContributorTeam>, IRepository
    {
        /// <summary>
        /// Gets all contributors team.
        /// </summary>
        /// <returns>Collection of contributors from the repository.</returns>
        ICollection<ContributorTeam> Find();
    }
}
