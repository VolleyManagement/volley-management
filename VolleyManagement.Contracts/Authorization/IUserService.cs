namespace VolleyManagement.Contracts
{
    /// <summary>
    /// Provides specified information about user.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get authorized user email.
        /// </summary>
        /// <returns>User email.</returns>
        string GetCurrentUserMailById();
    }
}
