namespace VolleyManagement.Data.Queries.User
{
    using Contracts;

    /// <summary>
    /// The find by name criteria.
    /// </summary>
    public class FindByNameCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets name of the entity to search
        /// </summary>
        public string Name { get; set; }
    }
}