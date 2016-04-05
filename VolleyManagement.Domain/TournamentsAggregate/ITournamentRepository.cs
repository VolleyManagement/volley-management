namespace VolleyManagement.Domain.TournamentsAggregate
{
    using VolleyManagement.Data.Contracts;

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
    }
}
