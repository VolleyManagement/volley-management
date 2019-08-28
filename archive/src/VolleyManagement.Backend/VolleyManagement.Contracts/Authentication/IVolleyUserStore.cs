namespace VolleyManagement.Contracts.Authentication
{
    using Microsoft.AspNet.Identity;

    using Models;

    /// <summary>
    /// Stores Volley Users data
    /// </summary>
    public interface IVolleyUserStore
        : IUserEmailStore<UserModel, int>,
          IUserLoginStore<UserModel, int>
    {
    }
}