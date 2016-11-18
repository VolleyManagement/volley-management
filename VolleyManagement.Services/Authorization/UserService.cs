namespace VolleyManagement.UI.Infrastructure
{
    using System;
    using Contracts;
    using Contracts.Exceptions;
    using Data.Contracts;
    using Data.Queries.Common;
    using Domain.UsersAggregate;

    /// <summary>
    /// Provides the way to get specified information about user.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IQuery<User, FindByIdCriteria> _getUserByIdQuery;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="getUserByIdQuery">Query for getting User by Id.</param>
        /// <param name="userRepository">The user repository.</param>
        public UserService(
            IQuery<User, FindByIdCriteria> getUserByIdQuery,
            IUserRepository userRepository)
        {
            this._getUserByIdQuery = getUserByIdQuery;
            this._userRepository = userRepository;
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
        /// <param name="userId">User Id.</param>
        public void SetUserBlocked(int userId)
        {
            bool isUserBlocked = true;
            SetBlockStatus(userId, isUserBlocked);
        }

        /// <summary>
        /// It unblocks an account pointed user.
        /// </summary>
        /// <param name="userId">User Id.</param>
        public void SetUserUnblocked(int userId)
        {
            bool isUserBlocked = false;
            SetBlockStatus(userId, isUserBlocked);
        }

        private void SetBlockStatus(int userId, bool isBlocked)
        {
            User user = GetUser(userId);
            if (user == null)
            {
                throw new MissingEntityException(Services.ServiceResources.ExceptionMessages.UserNotFound);
            }

            user.IsBlocked = isBlocked;
            _userRepository.Update(user);
            _userRepository.UnitOfWork.Commit();
        }
    }
}