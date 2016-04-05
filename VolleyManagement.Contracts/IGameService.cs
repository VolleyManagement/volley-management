namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;
    using VolleyManagement.Domain.GamesAggregate;

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
        /// Deletes game by its identifier.
        /// </summary>
        /// <param name="id">Identifier of game.</param>
        void Delete(int id);
    }
}
