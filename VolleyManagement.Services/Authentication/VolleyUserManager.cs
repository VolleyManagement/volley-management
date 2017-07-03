namespace VolleyManagement.Services.Authentication
{
    using System.Diagnostics.CodeAnalysis;
    using Contracts.Authentication;
    using Contracts.Authentication.Models;
    using Microsoft.AspNet.Identity;

    /// <summary>
    /// The volley user manager.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1063:ImplementIDisposableCorrectly",
        Justification = "Looks like false positive - it asks to remove IDisposable from list of implemented interfaces")]
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