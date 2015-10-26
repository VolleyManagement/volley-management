namespace VolleyManagement.Data.Queries.User
{
    using VolleyManagement.Data.Contracts;

    /// <summary>
    /// The find by name criteria.
    /// </summary>
    public class FindByEmailCriteria : IQueryCriteria
    {
        /// <summary>
        /// Email of the user to look for
        /// </summary>
        public string Email { get; set; }
    }
}