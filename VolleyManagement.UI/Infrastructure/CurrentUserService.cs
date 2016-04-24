namespace VolleyManagement.UI.Infrastructure
{
    using System.Web;
    using Contracts.Authorization;
    using Microsoft.AspNet.Identity;

    public class CurrentUserService : ICurrentUserService
    {
        private const int ANONIMOUS_USER_ID = -1;

        public int GetCurrentUserId()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated
                ? System.Convert.ToInt32(HttpContext.Current.User.Identity.GetUserId())
                : ANONIMOUS_USER_ID;
        }
    }
}