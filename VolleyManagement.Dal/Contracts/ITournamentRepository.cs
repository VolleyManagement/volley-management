namespace VolleyManagement.Dal.Contracts
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Defines specific contract for TournamentRepository
    /// </summary>
    public interface ITournamentRepository : IRepository<Tournament>
    {
        /// <summary>
        /// Gets all tournaments.
        /// </summary>
        /// <returns>Collection of tournaments from the repository.</returns>
        IQueryable<Tournament> FindAll();
    }
}
