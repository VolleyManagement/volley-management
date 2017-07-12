namespace VolleyManagement.Data.Queries.Group
{
    using Contracts;

    /// <summary>
    /// Provides criteria for getting all groups of tournament
    /// </summary>
    public class TournamentGroupsCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets target division Id
        /// </summary>
        public int DivisionId { get; set; }
    }
}
