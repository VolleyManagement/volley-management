namespace VolleyManagement.Domain.TournamentsAggregate
{
    using Data.Contracts;

    /// <summary>
    /// Defines specific contract for TournamentRepository
    /// </summary>
    public interface ITournamentRepository : IGenericRepository<Tournament>
    {
        /// <summary>
        /// Add team and group to tournament
        /// </summary>
        /// <param name="teamId">Team to add</param>
        /// <param name="tournamentId">Tournament to assign team</param>
        /// <param name="groupId">Group to add</param>
        /// <param name="divisionId">Division to assign group</param>
        void AddTeamToTournament(int teamId, int tournamentId, int groupId, int divisionId);

        /// <summary>
        /// Removes team from tournament
        /// </summary>
        /// <param name="teamId">Team to remove</param>
        /// <param name="tournamentId">Tournament to un assign team</param>
        void RemoveTeamFromTournament(int teamId, int tournamentId);
    }
}
