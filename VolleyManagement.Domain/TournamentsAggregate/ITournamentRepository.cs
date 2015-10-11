namespace VolleyManagement.Domain.TournamentsAggregate
{
    using System;
    using System.Linq;

    using VolleyManagement.Data.Contracts;

    /// <summary>
    /// Defines specific contract for TournamentRepository
    /// </summary>
    public interface ITournamentRepository : IRepository<Tournament>
    {
        /// <summary>
        /// Gets all tournaments.
        /// </summary>
        /// <returns>Collection of tournaments from the repository.</returns>
        [Obsolete("Use Object Query pattern")]
        IQueryable<Tournament> Find();
    }
}
