namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;
    using VolleyManagement.Domain.GameResultsAggregate;

    /// <summary>
    /// Defines a contract for GameResultService.
    /// </summary>
    public interface IGameResultService
    {
        /// <summary>
        /// Creates a new game result.
        /// </summary>
        /// <param name="gameResult">Game result to create.</param>
        void Create(GameResult gameResult);

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
        /// Edits specified instance of game result.
        /// </summary>
        /// <param name="gameResult">Game result to update.</param>
        void Edit(GameResult gameResult);

        /// <summary>
        /// Deletes game result by its identifier.
        /// </summary>
        /// <param name="id">Identifier of game result.</param>
        void Delete(int id);
    }
}
