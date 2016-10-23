namespace VolleyManagement.Contracts
{
    using Domain.UsersAggregate;

    /// <summary>
    /// Provides specified information about user.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get authorized user instance.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <returns>User instance.</returns>
        User GetUser(int userId);
    }
}
