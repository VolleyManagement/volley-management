namespace VolleyManagement.Data.Queries.Tournament
{
    using Contracts;

    /// <summary>
    /// Represents criteria for finding game results by given tournament criteria
    /// </summary>
    public class TournamentGamesCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets group id criteria
        /// </summary>
        public int TournamentId { get; set; }
    }
}
