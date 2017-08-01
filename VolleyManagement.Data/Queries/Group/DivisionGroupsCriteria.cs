namespace VolleyManagement.Data.Queries.Group
{
    using Contracts;

    /// <summary>
    /// Provides criteria for getting all groups
    /// of selected division in the tournament
    /// </summary>
    public class DivisionGroupsCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets division Id
        /// </summary>
        public int DivisionId { get; set; }
    }
}
