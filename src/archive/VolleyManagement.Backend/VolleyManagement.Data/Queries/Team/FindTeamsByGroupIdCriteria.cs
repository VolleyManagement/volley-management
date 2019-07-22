namespace VolleyManagement.Data.Queries.Team
{
    using Contracts;

    /// <summary>
    /// Criteria to find all teams from group with specified id.
    /// </summary>
    public class FindTeamsByGroupIdCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets Group ID to search for
        /// </summary>
        public int GroupId { get; set; }
    }
}
