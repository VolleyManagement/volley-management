namespace VolleyManagement.Data.Queries.Team
{
    using Contracts;

    public class FindTeamsByDivisionIdCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets Division Id to search for
        /// </summary>
        public int DivisionId { get; set; }
    }
}
