namespace VolleyManagement.Contracts.Authorization
{
    /// <summary>
    /// Provides the way to get information about current user
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Returns current user identifier
        /// </summary>
        /// <returns>Current user identifier</returns>
        int GetCurrentUserId();
    }
}
