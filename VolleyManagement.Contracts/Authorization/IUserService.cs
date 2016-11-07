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
        /// It blocks an account of unwished user.
        /// </summary>
        /// <param name="userId">User's Id.</param>
        void SetUserBlocked(int userId);

        /// <summary>
        /// It unblocks an account of pointed user.
        /// </summary>
        /// <param name="userId">User's Id.</param>
        void SetUserUnblocked(int userId);
    }
}
