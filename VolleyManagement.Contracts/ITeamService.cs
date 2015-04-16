namespace VolleyManagement.Contracts
{
    using System.Linq;
    using Domain.Teams;

    /// <summary>
    /// Interface for TeamService.
    /// </summary>
    public interface ITeamService
    {
        /// <summary>
        /// Gets list of all teams.
        /// </summary>
        /// <returns>Return list of all teams.</returns>
        IQueryable<Team> Get();

        /// <summary>
        /// Create new team.
        /// </summary>
        /// <param name="team">New team.</param>
        void Create(Team team);

        /// <summary>
        /// Delete team by id.
        /// </summary>
        /// <param name="id">Team id.</param>
        void Delete(int id);
    }
}
