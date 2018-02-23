namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;
    using System.Linq;

    using Domain.PlayersAggregate;
    using Domain.TeamsAggregate;

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
        /// <param name="playerToCreate">New player.</param>
        void Create(Player playerToCreate);

        /// <summary>
        /// Create new players.
        /// </summary>
        /// <param name="playersToCreate">New players.</param>
        void Create(List<Player> playersToCreate);

        /// <summary>
        /// Edit player profile.
        /// </summary>
        /// <param name="playerToEdit">Updated player data.</param>
        void Edit(Player playerToEdit);

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
