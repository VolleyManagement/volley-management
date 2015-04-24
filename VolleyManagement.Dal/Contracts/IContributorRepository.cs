namespace VolleyManagement.Dal.Contracts
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using VolleyManagement.Domain.Contributors;

    /// <summary>
    /// Defines specific contract for TournamentRepository
    /// </summary>
    public interface IContributorRepository : IRepository<Contributor>
    {
        /// <summary>
        /// Gets all contributors.
        /// </summary>
        /// <returns>Collection of contributors from the repository.</returns>
        IQueryable<Contributor> Find();
    }
}
