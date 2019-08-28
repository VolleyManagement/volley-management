namespace VolleyManagement.UI.Areas.Admin.Models
{
    using Domain.UsersAggregate;

    /// <summary>
    /// The authorization provider view model.
    /// </summary>
    public class LoginProviderViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginProviderViewModel"/> class.
        /// </summary>
        /// <param name="loginProvider">
        /// The login provider info.
        /// </param>
        public LoginProviderViewModel(LoginProviderInfo loginProvider)
        {
            LoginProvider = loginProvider.LoginProvider;
            ProviderKey = loginProvider.ProviderKey;
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