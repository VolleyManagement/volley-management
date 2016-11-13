namespace VolleyManagement.UI.Infrastructure
{
    using System;
    using Contracts;
    using Data.Contracts;
    using Data.Queries.Common;
    using Domain.UsersAggregate;

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

        /// <summary>
        /// It blocks an account of unwished user.
        /// </summary>
        /// <param name="userId"></param>
        public void SetUserBlocked(int userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// It unblocks an account pointed user.
        /// </summary>
        /// <param name="userId"></param>
        public void SetUserUnblocked(int userId)
        {
            throw new NotImplementedException();
        }
    }
}