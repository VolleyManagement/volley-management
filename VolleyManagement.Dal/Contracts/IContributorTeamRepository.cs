namespace VolleyManagement.Dal.Contracts
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using VolleyManagement.Domain.ContributorsAggregate;

    /// <summary>
    /// Defines specific contract for ContributorRepository
    /// </summary>
    public interface IContributorTeamRepository : IRepository<ContributorTeam>
    {
        /// <summary>
        /// Gets all contributors team.
        /// </summary>
        /// <returns>Collection of contributors from the repository.</returns>
        IQueryable<ContributorTeam> Find();
    }
}
