namespace VolleyManagement.Domain.TournamentsAggregate
{
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
        IQueryable<Tournament> Find();
    }
}
