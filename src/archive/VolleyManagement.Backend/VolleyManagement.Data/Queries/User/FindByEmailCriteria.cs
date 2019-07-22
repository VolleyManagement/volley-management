namespace VolleyManagement.Data.Queries.User
{
    using Contracts;

    /// <summary>
    /// The find by name criteria.
    /// </summary>
    public class FindByEmailCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets email of the user to look for
        /// </summary>
        public string Email { get; set; }
    }
}