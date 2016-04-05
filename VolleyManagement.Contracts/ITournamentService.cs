namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Domain.TournamentsAggregate;

    /// <summary>
    /// Interface for TournamentService
    /// </summary>
    public interface ITournamentService
    {
        /// <summary>
        /// Gets list of all tournaments
        /// </summary>
        /// <returns>Return list of all tournaments.</returns>
        List<Tournament> Get();

        /// <summary>
        /// Returns only actual tournaments
        /// </summary>
        /// <returns>Actual tournaments</returns>
        List<Tournament> GetActual();

        /// <summary>
        /// Returns only finished tournaments
        /// </summary>
        /// <returns>Finished tournaments</returns>
        List<Tournament> GetFinished();

        /// <summary>
        /// Find a Tournament by id
        /// </summary>
        /// <param name="id">id of Tournament to find</param>
        /// <returns>Found Tournament</returns>
        Tournament Get(int id);

        /// <summary>
        /// Returns all teams for specific tournament
        /// </summary>
        /// <param name="tournamentId">Id of Tournament for getting teams</param>
        /// <returns>Tournament teams</returns>
        List<Team> GetAllTournamentTeams(int tournamentId);
                            
        /// <summary>
        /// Create new tournament.
        /// </summary>
        /// <param name="tournament">New tournament</param>
        void Create(Tournament tournament);

        /// <summary>
        /// Edit tournament
        /// </summary>
        /// <param name="tournament">New tournament data</param>
        void Edit(Tournament tournament);

        /// <summary>
        /// Delete specific tournament
        /// </summary>
        /// <param name="id">Tournament id</param>
        void Delete(int id);

        /// <summary>
        /// Adds teams to tournament
        /// </summary>
        /// <param name="teams">Teams to add</param>
        /// <param name="tournamentId">Tournament to assign teams</param>
        void AddTeamsToTournament(IEnumerable<Team> teams, int tournamentId);

        /// <summary>
        /// Deletes team from tournament
        /// </summary>
        /// <param name="teamId">Team to delete</param>
        /// <param name="tournamentId">Tournament to un assign team</param>
        void DeleteTeamFromTournament(int teamId, int tournamentId);
    }
}
