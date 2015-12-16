namespace VolleyManagement.Data.Queries.GameResult
{
    using VolleyManagement.Data.Contracts;

    /// <summary>
    /// Represents criteria to retrieve tournament's game results by its identifier.
    /// </summary>
    public class TournamentGameResultsCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets the tournament's identifier.
        /// </summary>
        public int TournamentId { get; set; }
    }
}
