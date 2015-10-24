namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;

    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.TeamsAggregate;

    /// <summary>
    /// Interface for TeamService.
    /// </summary>
    public interface ITeamService
    {
        /// <summary>
        /// Gets list of all teams.
        /// </summary>
        /// <returns>Return list of all teams.</returns>
        List<Team> Get();

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
        /// <param name="teamId">Id of team which players should be found</param>
        /// <returns>Collection of team's players</returns>
        List<Player> GetTeamRoster(int teamId);

        /// <summary>
        /// Sets team to player
        /// </summary>
        /// <param name="playerId">Id of player to set the team</param>
        /// <param name="teamId">Id of team which should be set to player</param>
        void UpdatePlayerTeam(int playerId, int teamId);
    }
}
