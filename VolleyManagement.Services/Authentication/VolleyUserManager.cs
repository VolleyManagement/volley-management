namespace VolleyManagement.Services.Authentication
{
    using Microsoft.AspNet.Identity;

    using VolleyManagement.Contracts.Authentication;
    using VolleyManagement.Contracts.Authentication.Models;

    /// <summary>
    /// The volley user manager.
    /// </summary>
    public class VolleyUserManager : UserManager<UserModel, int>, IVolleyUserManager<UserModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VolleyUserManager"/> class.
        /// </summary>
        /// <param name="store">
        /// The store.
        /// </param>
        public VolleyUserManager(IVolleyUserStore store)
            : base(store)
        {
        }
    }
}