namespace VolleyManagement.Contracts
{
    using System.Linq;
    using Domain.Players;

    /// <summary>
    /// Interface for PlayerService.
    /// </summary>
    public interface IPlayerService
    {
        /// <summary>
        /// Gets list of all players.
        /// </summary>
        /// <returns>Return list of all players.</returns>
        IQueryable<Player> Get();

        /// <summary>
        /// Create new player.
        /// </summary>
        /// <param name="player">New player.</param>
        void Create(Player player);

        /// <summary>
        /// Edit player profile.
        /// </summary>
        /// <param name="player">Updated player data.</param>
        void Edit(Player player);

        /// <summary>
        /// Find player by id.
        /// </summary>
        /// <param name="id">Player id.</param>
        /// <returns>Found player.</returns>
        Player Get(int id);

        void Delete(int id);
    }
}
