namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.Domain.TournamentsAggregate.Standings;

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
        /// Gets standings of the tournament specified by identifier.
        /// </summary>
        /// <param name="id">Identifier of the tournament.</param>
        /// <returns>Standings of the tournament with specified identifier.</returns>
        Standings GetStandings(int id);
    }
}
