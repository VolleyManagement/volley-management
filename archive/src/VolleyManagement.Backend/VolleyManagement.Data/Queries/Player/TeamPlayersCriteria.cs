namespace VolleyManagement.Data.Queries.Player
{
    using Contracts;

    /// <summary>
    /// Provides parameters to retrieve Team roster
    /// </summary>
    public class TeamPlayersCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets Team ID to look for
        /// </summary>
        public int TeamId { get; set; }
    }
}