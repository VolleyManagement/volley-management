namespace VolleyManagement.Domain.TournamentsAggregate
{
    using Data.Contracts;

    /// <summary>
    /// Defines specific contract for TournamentRepository
    /// </summary>
    public interface ITournamentRepository : IGenericRepository<Tournament>
    {
        /// <summary>
        /// Adds team to tournament
        /// </summary>
        /// <param name="teamId">Team to add</param>
        /// <param name="tournamentId">Tournament to assign team</param>
        void AddTeamToTournament(int teamId, int tournamentId);

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
        /// <param name="divisionId">Division id from which remove group</param>
        void RemoveGroup(int groupId, int divisionId);

        /// <summary>
        /// Remove division
        /// </summary>
        /// <param name="divisionId">Division to be removed id</param>
        void RemoveDivision(int divisionId);
    }
}
