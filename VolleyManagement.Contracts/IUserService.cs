namespace VolleyManagement.Contracts
{
    using System.Linq;
    using Domain.Users;

    /// <summary>
    /// Interface for UserService
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets list of all users
        /// </summary>
        /// <returns>Return list of all users.</returns>
        IQueryable<User> GetAll();

        /// <summary>
        /// Create new user.
        /// </summary>
        /// <param name="user">New user</param>
        void Create(User user);

        /// <summary>
        /// Edit user profile
        /// </summary>
        /// <param name="user">Updated user data</param>
        void Edit(User user);

        /// <summary>
        /// Find user by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Found user</returns>
        User FindById(int id);
    }
}
