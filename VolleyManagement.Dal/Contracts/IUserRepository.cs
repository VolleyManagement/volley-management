namespace VolleyManagement.Dal.Contracts
{
    using System.Linq;
    using VolleyManagement.Domain.Users;

    /// <summary>
    /// Defines specific contract for UserRepository
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>Collection of users from the repository.</returns>
        IQueryable<User> Find();
    }
}
