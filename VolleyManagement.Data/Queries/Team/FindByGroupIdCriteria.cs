namespace VolleyManagement.Data.Queries.Team
{
    using Contracts;

    /// <summary>
    /// The find by group id criteria.
    /// </summary>
    public class FindByGroupIdCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets Captain ID to search for
        /// </summary>
        public int GroupId { get; set; }
    }
}
