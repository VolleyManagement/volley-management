namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;
    using Domain.GamesAggregate;

    /// <summary>
    /// Defines a contract for GameResultService.
    /// </summary>
    public interface IGameService
    {
        /// <summary>
        /// Creates a new game.
        /// </summary>
        /// <param name="game">Game to create.</param>
        void Create(Game game);

        /// <summary>
        /// Gets game result by its identifier.
        /// </summary>
        /// <param name="id">Identifier of game result.</param>
        /// <returns>Instance of <see cref="GameResultDto"/> or null if nothing is obtained.</returns>
        GameResultDto Get(int id);

        /// <summary>
        /// Gets game results of the tournament specified by its identifier.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>List of game results of specified tournament.</returns>
        List<GameResultDto> GetTournamentResults(int tournamentId);

        /// <summary>
        /// Edits specified instance of game.
        /// </summary>
        /// <param name="game">Game to update.</param>
        void Edit(Game game);

        /// <summary>
        /// Edits result of specified instance of game.
        /// </summary>
        /// <param name="game">Game which result have to be to update.</param>
        void EditGameResult(Game game);

        /// <summary>
        /// Deletes game by its identifier.
        /// </summary>
        /// <param name="id">Identifier of game.</param>
        void Delete(int id);

        /// <summary>
        /// Swap all games between two rounds.
        /// </summary>
        /// <param name="tournamentId"> Identifier of tournament.</param>
        /// <param name="firstRoundNumber"> Identifier of first round.</param>
        /// <param name="secondRoundNumber"> Identifier of second round.</param>
        void SwapRounds(int tournamentId, byte firstRoundNumber, byte secondRoundNumber);

        /// <summary>
        /// Deletes all games in tournament.
        /// </summary>
        /// <param name="tournamentId">The id of tournament from which to remove games.</param>
        void RemoveAllGamesInTournament(int tournamentId);

        /// <summary>
        /// Add collection of games into tournament
        /// </summary>
        /// <param name="games">Collection of games</param>
        void AddGames(List<Game> games);
    }
}
