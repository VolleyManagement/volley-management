namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;
    using Contracts;
    using Domain.TeamsAggregate;
    using Domain.TournamentsAggregate;

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
        /// Gets tournament by its group
        /// </summary>
        /// <param name="groupId">id of group </param>
        /// <returns>Return current tournament.</returns>
        Tournament GetTournamentByGroup(int groupId);

        /// <summary>
        /// Returns only actual tournaments
        /// </summary>
        /// <returns>Actual tournaments</returns>
        List<Tournament> GetActual();

        /// <summary>
        /// Returns only archived tournaments
        /// </summary>
        /// <returns>Archived tournaments</returns>
        List<Tournament> GetArchived();

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
        /// Returns all divisions for specific tournament
        /// </summary>
        /// <param name="tournamentId">Id of Tournament to get divisions</param>
        /// <returns>Tournament divisions</returns>
        List<Division> GetAllTournamentDivisions(int tournamentId);

        /// <summary>
        /// Returns all groups for specific tournament by division id
        /// </summary>
        /// <param name="divisionId">Id of Division to get groups</param>
        /// <returns>Tournament groups</returns>
        List<Group> GetAllTournamentGroups(int divisionId);

        /// <summary>
        /// Finds tournament data transfer object by tournament id
        /// </summary>
        /// <param name="tournamentId">Tournament id</param>
        /// <returns>The <see cref="TournamentScheduleDto"/></returns>
        TournamentScheduleDto GetTournamentScheduleInfo(int tournamentId);

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
        /// Archive specific tournament
        /// </summary>
        /// <param name="id">Tournament id</param>
        void Archive(int id);

        /// <summary>
        /// Adds selected teams to tournament
        /// </summary>
        /// <param name="groupTeam">Teams related to specific groups that will be added to tournament</param>
        void AddTeamsToTournament(List<TeamTournamentAssignmentDto> groupTeam);

        /// <summary>
        /// Deletes team from tournament
        /// </summary>
        /// <param name="teamId">Team to delete</param>
        /// <param name="tournamentId">Tournament to un assign team</param>
        void DeleteTeamFromTournament(int teamId, int tournamentId);

        /// <summary>
        /// Counts number of rounds for specified tournament
        /// </summary>
        /// <param name="tournament">Tournament for which we count rounds</param>
        /// <returns>Number of rounds</returns>
        byte GetNumberOfRounds(TournamentScheduleDto tournament);

        /// <summary>
        /// Returns all teams that don't take part in specific tournament
        /// </summary>
        /// <param name="tournamentId">Id of Tournament for getting teams</param>
        /// <returns>Teams that don't take part in tournament</returns>
        IEnumerable<Team> GetAllNoTournamentTeams(int tournamentId);
    }
}
