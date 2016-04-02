namespace VolleyManagement.Data.Queries.Team
{
    using VolleyManagement.Data.Contracts;

    /// <summary>
    /// Criteria to find all teams from tournament with specified id
    /// </summary>
    public class FindByTournamentIdCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets Tournament ID to search for
        /// </summary>
        public int TournamentId { get; set; }
    }
}
