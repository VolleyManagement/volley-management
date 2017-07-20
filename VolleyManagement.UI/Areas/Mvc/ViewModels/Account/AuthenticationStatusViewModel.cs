namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Account
{
    /// <summary>
    /// Provides Authentication information for current user
    /// </summary>
    public class AuthenticationStatusViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether is User Authenticated
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// Gets or sets name of the user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets url to return after login/logout
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}