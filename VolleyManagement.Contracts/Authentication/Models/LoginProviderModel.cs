namespace VolleyManagement.Contracts.Authentication.Models
{
    /// <summary>
    /// Provides login provider information
    /// </summary>
    public class LoginProviderModel
    {
        /// <summary>
        /// Provider for the linked login, i.e. Facebook, Google, etc.
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// User specific key for the login provider
        /// </summary>
        public string ProviderKey { get; set; }
    }
}