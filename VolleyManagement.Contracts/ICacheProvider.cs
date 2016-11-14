namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;
    using VolleyManagement.Domain.UsersAggregate;

    /// <summary>
    /// Helps to control active users
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Gets user by its index.
        /// </summary>
        /// <param name="value">Key for application.</param>
        /// <returns>List of all users id.</returns>
        List<int> this[string value] { get; }
    }
}
