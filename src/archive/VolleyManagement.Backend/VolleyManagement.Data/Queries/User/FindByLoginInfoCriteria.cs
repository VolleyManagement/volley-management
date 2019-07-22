namespace VolleyManagement.Data.Queries.User
{
    using Contracts;

    /// <summary>
    /// The find by name criteria.
    /// </summary>
    public class FindByLoginInfoCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets name of the login provider
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets key assigned to user by provider
        /// </summary>
        public string ProviderKey { get; set; }
    }
}