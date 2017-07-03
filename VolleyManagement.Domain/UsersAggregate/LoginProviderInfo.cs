namespace VolleyManagement.Domain.UsersAggregate
{
    /// <summary>
    /// Stores external login data for user
    /// </summary>
    public class LoginProviderInfo
    {
        /// <summary>
        /// Gets or sets key retrieved from provider
        /// </summary>
        public string ProviderKey { get; set; }

        /// <summary>
        /// Gets or sets provider name
        /// </summary>
        public string LoginProvider { get; set; }
    }
}