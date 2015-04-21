namespace VolleyManagement.Dal.Contracts
{
    using System.Linq;
    using VolleyManagement.Domain.Teams;

    /// <summary>
    /// Defines specific contract for PlayerRepository
    /// </summary>
    public interface ITeamRepository : IRepository<Team>
    {
        /// <summary>
        /// Gets all teams.
        /// </summary>
        /// <returns>Collection of teams from the repository.</returns>
        IQueryable<Team> Find();
    }
}
