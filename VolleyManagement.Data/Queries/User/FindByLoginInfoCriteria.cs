namespace VolleyManagement.Data.Queries.User
{
    using VolleyManagement.Data.Contracts;

    /// <summary>
    /// The find by name criteria.
    /// </summary>
    public class FindByLoginInfoCriteria : IQueryCriteria
    {
        /// <summary>
        /// Name of the login provider
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// Key assigned to user by provider
        /// </summary>
        public string ProviderKey { get; set; }
    }
}