namespace VolleyManagement.Services.Authorization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Data.Contracts;
    using Data.Queries.Common;
    using Data.Queries.User;
    using Domain.PlayersAggregate;
    using Domain.RolesAggregate;
    using Domain.UsersAggregate;

    /// <summary>
    /// Provides the way to get specified information about user.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IAuthorizationService _authService;
        private readonly IQuery<User, FindByIdCriteria> _getUserByIdQuery;
        private readonly IUserRepository _userRepository;
        private readonly IQuery<ICollection<User>, GetAllCriteria> _getAllUsersQuery;
        private readonly IQuery<Player, FindByIdCriteria> _getUserPlayerQuery;
        private readonly ICacheProvider _cacheProvider;
        private readonly IQuery<ICollection<User>, UniqueUserCriteria> _getAdminsListQuery;
        private readonly ICurrentUserService _currentUserService;

#pragma warning disable S107 // Methods should not have too many parameters
        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="authService">Authorization service</param>
        /// <param name="getUserByIdQuery">Query for getting User by Id.</param>
        /// <param name="getAllUsersQuery">Query for getting all User.</param>
        /// <param name="getUserPlayerQuery">Query for getting player assigned to User</param>
        /// <param name="cacheProvider">Instance of <see cref="ICacheProvider"/> class.</param>
        /// <param name="getAdminsListQuery">Query for getting list of admins.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="currentUserService">Instance of <see cref="ICurrentUserService"/> class.</param>
        public UserService(
            IAuthorizationService authService,
            IQuery<User, FindByIdCriteria> getUserByIdQuery,
            IQuery<ICollection<User>, GetAllCriteria> getAllUsersQuery,
            IQuery<Player, FindByIdCriteria> getUserPlayerQuery,
            ICacheProvider cacheProvider,
            IQuery<ICollection<User>, UniqueUserCriteria> getAdminsListQuery,
            IUserRepository userRepository,
            ICurrentUserService currentUserService)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            _authService = authService;
            _getUserByIdQuery = getUserByIdQuery;
            _getAllUsersQuery = getAllUsersQuery;
            _getUserPlayerQuery = getUserPlayerQuery;
            _cacheProvider = cacheProvider;
            _getAdminsListQuery = getAdminsListQuery;
            _userRepository = userRepository;
            _currentUserService = currentUserService;
        }

        private int CurrentUserId => _currentUserService.GetCurrentUserId();

        /// <summary>
        /// Gets User entity by Id.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <returns>User entity.</returns>
        public User GetUser(int userId)
        {
            return _getUserByIdQuery.Execute(
                new FindByIdCriteria { Id = userId });
        }

        /// <summary>
        /// Gets User entity by Id.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <returns>User entity.</returns>
        public User GetUserDetails(int userId)
        {
            _authService.CheckAccess(AuthOperations.AllUsers.ViewDetails);
            var user = GetUser(userId);
            if (user != null)
            {
                user.Player = GetPlayer(user.PlayerId.GetValueOrDefault());
            }

            return user;
        }

        /// <summary>
        /// Get all users collection.
        /// </summary>
        /// <returns>Use collection.</returns>
        public ICollection<User> GetAllUsers()
        {
            _authService.CheckAccess(AuthOperations.AllUsers.ViewList);
            return _getAllUsersQuery.Execute(new GetAllCriteria());
        }

        /// <summary>
        /// Get all users collection.
        /// </summary>
        /// <returns>Use collection.</returns>
        public ICollection<User> GetAllActiveUsers()
        {
            _authService.CheckAccess(AuthOperations.AllUsers.ViewActiveList);
            var activeUsersList = _cacheProvider["ActiveUsers"] as List<int> ?? new List<int>();
            _cacheProvider["ActiveUsers"] = activeUsersList;
            return activeUsersList.Select(GetUser).ToList();
        }

        /// <summary>
        /// Gets list of users which role is Admin.
        /// </summary>
        /// <returns>List of User entities.</returns>
        public ICollection<User> GetAdminsList()
        {
            return _getAdminsListQuery.Execute(
                new UniqueUserCriteria { RoleId = 1 });
        }

        /// <summary>
        /// block or unblock user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="toBlock">set user block or not</param>
        public void ChangeUserBlocked(int userId, bool toBlock)
        {
            if (userId == CurrentUserId)
            {
                throw new InvalidOperationException(ServiceResources.ExceptionMessages.UserBlockHimself);
            }

            User user = GetUser(userId);
            if (user == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.UserNotFound);
            }

            user.IsBlocked = toBlock;
            _userRepository.Update(user);
            _userRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Get player instance by Id.
        /// </summary>
        /// <param name="playerId">Player Id.</param>
        /// <returns>Player instance.</returns>
        private Player GetPlayer(int playerId)
        {
            return _getUserPlayerQuery.Execute(new FindByIdCriteria { Id = playerId });
        }
    }
}