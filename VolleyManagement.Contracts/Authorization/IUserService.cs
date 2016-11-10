namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;
    using Domain.UsersAggregate;

    /// <summary>
    /// Provides specified information about user.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get all registered users.
        /// </summary>
        /// <returns>List of users.</returns>
        List<User> GetAllUsers();

        /// <summary>
        /// Get user instance by Id.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <returns>User entity.</returns>
        User GetUser(int userId);

        /// <summary>
        /// Get user instance by Id.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <returns>User instance.</returns>
        User GetUserDetails(int userId);

        /// <summary>
        /// Gets list of users which role is Admin.
        /// </summary>
        /// <returns>List of User instances.</returns>
        List<User> GetAdminsList();
    }
}
