namespace VolleyManagement.Contracts
{
    using System.Linq;

    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.TeamsAggregate;

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

        /// <summary>
        /// Delete player by id.
        /// </summary>
        /// <param name="id">Player id.</param>
        void Delete(int id);

        /// <summary>
        /// Find team of specified player
        /// </summary>
        /// <param name="player">Player which team should be found</param>
        /// <returns>Player's team</returns>
        Team GetPlayerTeam(Player player);
    }
}
