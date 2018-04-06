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
        ICollection<Team> Get();

        /// <summary>
        /// Find team by id.
        /// </summary>
        /// <param name="id">Team id.</param>
        /// <returns>Found team.</returns>
        Team Get(int id);

        /// <summary>
        /// Create new team.
        /// </summary>
        /// <param name="teamToCreate">New team.</param>
        Team Create(Team teamToCreate);

        /// <summary>
        /// Edit team.
        /// </summary>
        /// <param name="teamToEdit">Team to edit.</param>
        void Edit(Team teamToEdit);

        /// <summary>
        /// Delete team by id.
        /// </summary>
        /// <param name="teamId">Team id.</param>
        void Delete(int teamId);

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
        ICollection<Player> GetTeamRoster(int teamId);

        /// <summary>
        /// Sets team id to roster
        /// </summary>
        /// <param name="roster">Players to set the team id</param>
        /// <param name="teamId">Id of team which should be set to player</param>
        void UpdateRosterTeamId(ICollection<Player> roster, int teamId);

        /// <summary>
        /// Changes captain of the team.
        /// </summary>
        /// <param name="captainId">Id of captain.</param>
        /// <param name="team">Team, which captain to change.</param>
        void ChangeCaptain(PlayerId captainId, Team team);

        /// <summary>
        /// Addes teams to team.
        /// </summary>
        /// <param name="players">List of players.</param>
        /// <param name="team">Team.</param>
        void AddPlayers(IEnumerable<PlayerId> players, Team team);

        /// <summary>
        /// Removes teams to team.
        /// </summary>
        /// <param name="players">List of players.</param>
        /// <param name="team">Team.</param>
        void RemovePlayers(IEnumerable<PlayerId> players, Team team);
    }
}
