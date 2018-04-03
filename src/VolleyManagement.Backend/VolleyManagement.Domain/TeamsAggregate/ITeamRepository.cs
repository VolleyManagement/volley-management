namespace VolleyManagement.Domain.TeamsAggregate
{
    using Data.Contracts;

    /// <summary>
    /// Defines specific contract for PlayerRepository
    /// </summary>
    public interface ITeamRepository
    {
        /// <summary>
        /// Adds new team.
        /// </summary>
        /// <param name="newEntity">The team for adding.</param>
        Team Add(Team newEntity);

        /// <summary>
        /// Updates specified team.
        /// </summary>
        /// <param name="updatedEntity">Updated team.</param>
        void Update(Team updatedEntity);

        /// <summary>
        /// Removes team by id.
        /// </summary>
        /// <param name="id">The id of team to remove.</param>
        void Remove(int id);
    }
}