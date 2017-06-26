namespace VolleyManagement.Data.Queries.Division
{
    using Contracts;

    /// <summary>
    /// Provides criteria for getting all divisions of t
    /// </summary>
    public class TournamentDivisionsCriteria : IQueryCriteria
    {
        /// <summary>
        /// Target tournament Id
        /// </summary>
        public int TournamentId { get; set; }
    }
}
