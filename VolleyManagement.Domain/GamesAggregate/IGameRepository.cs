namespace VolleyManagement.Domain.GamesAggregate
{
    using System.Collections.Generic;
    using VolleyManagement.Data.Contracts;

    /// <summary>
    /// Defines a contract for GameRepository.
    /// </summary>
    public interface IGameRepository : IGenericRepository<Game>
    {
        /// <summary>
        /// Deletes all games in tournament.
        /// </summary>
        /// <param name="tournamentId">The id of tournament from which to remove games.</param>
        void RemoveAllGamesInTournament(int tournamentId);

        /// <summary>
        /// Add collection of games into tournament
        /// </summary>
        /// <param name="games">Collection of games to be added</param>
        void AddGamesInTournament(List<Game> games);
    }
}