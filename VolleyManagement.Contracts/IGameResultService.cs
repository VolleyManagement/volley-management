namespace VolleyManagement.Contracts
{
    using VolleyManagement.Domain.GameResultsAggregate;

    /// <summary>
    /// Defines a contract for GameResultService.
    /// </summary>
    public interface IGameResultService
    {
        /// <summary>
        /// Creates new game result.
        /// </summary>
        /// <param name="gameResult">Domain model of game result.</param>
        void Create(GameResult gameResult);
    }
}
