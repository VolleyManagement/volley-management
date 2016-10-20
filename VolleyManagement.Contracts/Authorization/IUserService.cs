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
        /// <returns>User instance.</returns>
        User GetCurrentUserInstance();
    }
}
