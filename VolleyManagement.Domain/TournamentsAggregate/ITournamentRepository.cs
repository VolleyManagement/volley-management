namespace VolleyManagement.Domain.TournamentsAggregate
{
    using Data.Contracts;

    /// <summary>
    /// Defines specific contract for TournamentRepository
    /// </summary>
    public interface ITournamentRepository : IGenericRepository<Tournament>
    {
        /// <summary>
        /// Adds selected teams to tournament
        /// </summary>
        /// <param name="teamId">Teams that will be added to tournament</param>
        /// <param name="groupId">Groups of tournament to assign to team</param>
        void AddTeamToTournament(int teamId, int groupId);

        /// <summary>
        /// Removes team from tournament
        /// </summary>
        /// <param name="teamId">Team to remove</param>
        /// <param name="tournamentId">Tournament to un assign team</param>
        void RemoveTeamFromTournament(int teamId, int tournamentId);

        /// <summary>
        /// Removes group
        /// </summary>
        /// <param name="groupId">Group to be removed id</param>
        void RemoveGroup(int groupId);

        /// <summary>
        /// Remove division
        /// </summary>
        /// <param name="divisionId">Division to be removed id</param>
        void RemoveDivision(int divisionId);
    }
}
