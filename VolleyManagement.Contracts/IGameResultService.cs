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
        /// Gets all game results.
        /// </summary>
        /// <returns>List of all game results.</returns>
        List<GameResult> Get();

        /// <summary>
        /// Finds game result by specified identifier.
        /// </summary>
        /// <param name="id">Identifier of game result.</param>
        /// <returns>Instance of <see cref="GameResult"/> or null if nothing is found.</returns>
        GameResult Get(int id);

        /// <summary>
        /// Creates new game result.
        /// </summary>
        /// <param name="gameResult">Domain model of game result.</param>
        void Create(GameResult gameResult);
    }
}
