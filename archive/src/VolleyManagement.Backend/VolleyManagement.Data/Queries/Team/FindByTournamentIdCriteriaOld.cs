namespace VolleyManagement.Data.Queries.Team
{
    using System;
    using Contracts;

    /// <summary>
    /// Criteria to find all teams from tournament with specified id
    /// </summary>
    [Obsolete("Use "+nameof(FindByTournamentIdCriteria)+" based Query")]
    public class FindByTournamentIdCriteriaOld : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets Tournament ID to search for
        /// </summary>
        public int TournamentId { get; set; }
    }
}
