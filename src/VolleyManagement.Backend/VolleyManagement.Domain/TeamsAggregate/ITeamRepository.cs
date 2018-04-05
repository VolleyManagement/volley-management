namespace VolleyManagement.Domain.TeamsAggregate
{
    using Data.Contracts;
    using System.Collections.Generic;

    /// <summary>
    /// Defines specific contract for PlayerRepository
    /// </summary>
    public interface ITeamRepository
    {
        /// <summary>
        /// Adds new team to database.
        /// </summary>
        /// <param name="name">Name of the team</param>
        /// <param name="coach">Team's coach</param>
        /// <param name="captain">Captain's id</param>
        /// <param name="achievements">Achievements</param>
        /// <param name="roster">Collection of id of team's members.</param>
        /// <returns>Team representation of entity.</returns>
        Team Add(string name, string coach, PlayerId captain, string achievements, ICollection<PlayerId> roster);

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