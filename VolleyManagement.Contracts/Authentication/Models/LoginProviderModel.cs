namespace VolleyManagement.Contracts.Authentication.Models
{
    /// <summary>
    /// Provides login provider information
    /// </summary>
    public class LoginProviderModel
    {
        /// <summary>
        /// Gets or sets provider for the linked login, i.e. Facebook, Google, etc.
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets user specific key for the login provider
        /// </summary>
        public string ProviderKey { get; set; }
    }
}