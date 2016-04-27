namespace VolleyManagement.UI.Infrastructure
{
    using System.Web;
    using Contracts.Authorization;
    using Microsoft.AspNet.Identity;

    /// <summary>
    /// Provides the way to get information about current user
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private const int ANONYM = -1;

        /// <summary>
        /// Returns current user identifier
        /// </summary>
        /// <returns>Current user identifier</returns>
        public int GetCurrentUserId()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated
                ? System.Convert.ToInt32(HttpContext.Current.User.Identity.GetUserId())
                : ANONYM;
        }
    }
}