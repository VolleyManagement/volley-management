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
        /// Gets or sets user by its index.
        /// </summary>
        /// <param name="key">Key for application.</param>
        /// <returns>List of all users id.</returns>
        object this[string key] { get; set; }
    }
}
