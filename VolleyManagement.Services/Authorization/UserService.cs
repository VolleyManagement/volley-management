namespace VolleyManagement.UI.Infrastructure
{
    using System.Collections.Generic;
    using Contracts;
    using Data.Contracts;
    using Data.Queries.Common;
    using Domain.UsersAggregate;

    using VolleyManagement.Domain.RolesAggregate;

    /// <summary>
    /// Provides the way to get specified information about user.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IQuery<User, FindByIdCriteria> _getUserByIdQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="getUserByIdQuery">Query for getting User by Id.</param>
        public UserService(IQuery<User, FindByIdCriteria> getUserByIdQuery)
        {
            this._getUserByIdQuery = getUserByIdQuery;
        }

        /// <summary>
        /// Gets User entity by Id.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <returns>User entity.</returns>
        public User GetUser(int userId)
        {
            return this._getUserByIdQuery.Execute(
                    new FindByIdCriteria { Id = userId });
        }

        public List<User> GetAllUsers()
        {
            return null;
        }

        public List<LoginProviderInfo> GetAuthProviders(int userId)
        {
            return null;
        }

        public List<Role> GetUserRoles(int userId)
        {
            return null;
        }
    }
}