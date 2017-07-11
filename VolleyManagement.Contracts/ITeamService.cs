namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;

    using Domain.PlayersAggregate;
    using Domain.TeamsAggregate;

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
        /// <param name="name">Name of team for create.</param>
        void Create(Team team, string name);

        /// <summary>
        /// Edit team.
        /// </summary>
        /// <param name="team">Team to edit.</param>
        void Edit(Team team);

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
        /// Sets team id to roster
        /// </summary>
        /// <param name="roster">Players to set the team id</param>
        /// <param name="teamId">Id of team which should be set to player</param>
        void UpdateRosterTeamId(List<Player> roster, int teamId);
    }
}
