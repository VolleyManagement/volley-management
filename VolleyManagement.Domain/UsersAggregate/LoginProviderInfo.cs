namespace VolleyManagement.Domain.UsersAggregate
{
    /// <summary>
    /// Stores external login data for user
    /// </summary>
    public class LoginProviderInfo
    {
        /// <summary>
        /// Key retrieved from provider
        /// </summary>
        public string ProviderKey { get; set; }

        /// <summary>
        /// Provider name
        /// </summary>
        public string LoginProvider { get; set; }
    }
}