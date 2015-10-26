namespace VolleyManagement.Data.Queries.User
{
    using VolleyManagement.Data.Contracts;

    /// <summary>
    /// The find by name criteria.
    /// </summary>
    public class FindByNameCriteria : IQueryCriteria
    {
        /// <summary>
        /// Name of the entity to search
        /// </summary>
        public string Name { get; set; }
    }
}