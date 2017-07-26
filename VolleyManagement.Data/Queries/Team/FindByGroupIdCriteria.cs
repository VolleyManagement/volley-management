namespace VolleyManagement.Data.Queries.Team
{
    using Contracts;

    /// <summary>
    /// The find by group id criteria.
    /// </summary>
    public class FindByGroupIdCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets Group ID to search for
        /// </summary>
        public int GroupId { get; set; }
    }
}
