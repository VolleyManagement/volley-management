namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Account
{
    /// <summary>
    /// Provides Authentication information for current user
    /// </summary>
    public class AuthenticationStatusViewModel
    {
        /// <summary>
        /// Is User Authenticated
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// Name of the user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Url to return after login/logout
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}