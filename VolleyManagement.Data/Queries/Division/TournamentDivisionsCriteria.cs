namespace VolleyManagement.Data.Queries.Division
{
    using Contracts;

    /// <summary>
    /// Provides criteria for getting all divisions of tournament
    /// </summary>
    public class TournamentDivisionsCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets target tournament Id
        /// </summary>
        public int TournamentId { get; set; }
    }
}
