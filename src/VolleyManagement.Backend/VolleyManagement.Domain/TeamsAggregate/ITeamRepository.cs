namespace VolleyManagement.Domain.TeamsAggregate
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines specific contract for PlayerRepository
    /// </summary>
    public interface ITeamRepository
    {
        /// <summary>
        /// Adds new team to database.
        /// </summary>
        /// <param name="teamToCreate">All needed data to create new entity of team.</param>
        /// <returns>Team representation of entity.</returns>
        Team Add(CreateTeamDto teamToCreate);

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