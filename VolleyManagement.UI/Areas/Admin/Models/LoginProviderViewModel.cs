namespace VolleyManagement.UI.Areas.Admin.Models
{
    using VolleyManagement.Domain.UsersAggregate;

    /// <summary>
    /// The auth provider view model.
    /// </summary>
    public class LoginProviderViewModel
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginProviderViewModel"/> class.
        /// </summary>
        /// <param name="authProvider">
        /// The auth provider info.
        /// </param>
        public LoginProviderViewModel(LoginProviderInfo authProvider)
        {
            LoginProvider = authProvider.LoginProvider;
            ProviderKey = authProvider.ProviderKey;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string ProviderKey { get; set; }
    }
}