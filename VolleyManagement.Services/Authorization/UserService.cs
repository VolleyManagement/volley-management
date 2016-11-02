namespace VolleyManagement.UI.Infrastructure
{
    using System.Collections.Generic;
    using Contracts;
    using Data.Contracts;
    using Data.Queries.Common;
    using Domain.UsersAggregate;
    using VolleyManagement.Data.Queries.User;

    /// <summary>
    /// Provides the way to get specified information about user.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IQuery<User, FindByIdCriteria> _getUserByIdQuery;

        private readonly IQuery<List<User>, UniqueUserCriteria> _getAdminsListQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="getUserByIdQuery">Query for getting User by Id.</param>
        /// /// <param name="getAdminsListQuery">Query for getting list of admins.</param>
        public UserService(
            IQuery<User, FindByIdCriteria> getUserByIdQuery,
            IQuery<List<User>, UniqueUserCriteria> getAdminsListQuery)
        {
            this._getUserByIdQuery = getUserByIdQuery;
            this._getAdminsListQuery = getAdminsListQuery;
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

        /// <summary>
        /// Gets list of users which role is Admin.
        /// </summary>
        /// <returns>List of User entities.</returns>
        public List<User> GetAdminsList()
        {
            return _getAdminsListQuery.Execute(
                new UniqueUserCriteria { RoleId = 1 });
        }
    }
}