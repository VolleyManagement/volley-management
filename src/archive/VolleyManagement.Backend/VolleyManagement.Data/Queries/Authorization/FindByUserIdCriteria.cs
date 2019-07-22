namespace VolleyManagement.Data.Queries.Common
{
    using Contracts;

    /// <summary>
    /// Get By Id type queries parameters
    /// </summary>
    public class FindByUserIdCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets user Id
        /// </summary>
        public int UserId { get; set; }
    }
}