namespace VolleyManagement.UI.Infrastructure
{
    using System.Threading.Tasks;
    using System.Web;

    using Areas.Mvc.ViewModels.Users;
    using Contracts;
    using Contracts.Authentication;
    using Contracts.Authentication.Models;
    using Microsoft.AspNet.Identity;

    /// <summary>
    /// Provides the way to get specified information about user.
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// Holds instance of <see cref="IVolleyUserStore"/> contract.
        /// </summary>
        private readonly IVolleyUserStore _userStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userStore">Instance of the class
        /// that implements <see cref="IVolleyUserStore"/></param> interface.
        public UserService(IVolleyUserStore userStore)
        {
            this._userStore = userStore;
        }

        /// <summary>
        /// Get authorized user email.
        /// </summary>
        /// <returns>User email.</returns>
        public string GetCurrentUserMailById()
        {
            if (HttpContext.Current.User != null
                && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                int currentUserId = int.Parse(
                        HttpContext.Current.User.Identity.GetUserId());
                var userTask =
                    Task.Run(() => this._userStore.FindByIdAsync(currentUserId));
                UserModel user = userTask.Result;
                UserViewModel userViewModel = UserViewModel.Map(user);
                return userViewModel.Email;
            }

            return string.Empty;
        }
    }
}
