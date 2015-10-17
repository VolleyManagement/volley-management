namespace VolleyManagement.Data.Queries.Team
{
    using VolleyManagement.Data.Contracts;

    /// <summary>
    /// The find by captain id criteria.
    /// </summary>
    public class FindByCaptainIdCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets Captain ID to search for
        /// </summary>
        public int CaptainId { get; set; }
    }
}