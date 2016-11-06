namespace VolleyManagement.Contracts
{
    using Domain.UsersAggregate;

    /// <summary>
    /// Provides specified information about user.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get user instance by Id.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <returns>User instance.</returns>
        User GetUser(int userId);

        /// <summary>
        /// It blocks a dangerous user.
        /// </summary>
        /// <param name="userId">User's Id.</param>
        void SetUserBlocked(int userId);

        /// <summary>
        /// It unblocks a user.
        /// </summary>
        /// <param name="userId">User's Id.</param>
        void SetUserUnblocked(int userId);
    }
}
