namespace VolleyManagement.Domain.TeamsAggregate
{
    using System.Linq;

    using VolleyManagement.Data.Contracts;

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