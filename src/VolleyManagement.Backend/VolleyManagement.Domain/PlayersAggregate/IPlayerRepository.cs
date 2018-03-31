namespace VolleyManagement.Domain.PlayersAggregate
{
    using Data.Contracts;

    /// <summary>
    /// Defines specific contract for PlayerRepository
    /// </summary>
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        /// <summary>
        /// Update the player teamId to the DB.
        /// </summary>
        /// <param name="updatedEntity">Updated element.</param>
        void UpdateTeam(Player updatedEntity, int? teamId);
    }
}
