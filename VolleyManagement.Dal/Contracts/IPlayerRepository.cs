namespace VolleyManagement.Dal.Contracts
{
    using System.Linq;
    using VolleyManagement.Domain.Players;

    /// <summary>
    /// Defines specific contract for PlayerRepository
    /// </summary>
    public interface IPlayerRepository : IRepository<Player>
    {
        /// <summary>
        /// Gets all players.
        /// </summary>
        /// <returns>Collection of players from the repository.</returns>
        IQueryable<Player> Find();
    }
}
