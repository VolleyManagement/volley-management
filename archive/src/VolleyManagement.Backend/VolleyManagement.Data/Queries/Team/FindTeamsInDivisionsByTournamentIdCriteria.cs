namespace VolleyManagement.Data.Queries.Team
{
    using Contracts;

    public class FindTeamsInDivisionsByTournamentIdCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets Tournament Id to search for
        /// </summary>
        public int TournamentId { get; set; }
    }
}
