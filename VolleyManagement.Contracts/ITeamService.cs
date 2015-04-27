namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Players;
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
        /// Find team by id.
        /// </summary>
        /// <param name="id">Team id.</param>
        /// <returns>Found team.</returns>
        Team Get(int id);

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

        /// <summary>
        /// Find captain of specified team
        /// </summary>
        /// <param name="team">Team which captain should be found</param>
        /// <returns>Team's captain</returns>
        Player GetTeamCaptain(Team team);

        /// <summary>
        /// Find players of specified team
        /// </summary>
        /// <param name="team">Team which players should be found</param>
        /// <returns>Collection of team's players</returns>
        IEnumerable<Player> GetTeamRoster(Team team);
    }
}
