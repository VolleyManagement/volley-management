namespace VolleyManagement.Services.Authorization
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Contracts.Exceptions;
    using Data.Contracts;
    using Data.Queries.Common;
    using Domain.UsersAggregate;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Data.Queries.User;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.RolesAggregate;

    /// <summary>
    /// Provides the way to get specified information about user.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IAuthorizationService _authService;
        private readonly IQuery<User, FindByIdCriteria> _getUserByIdQuery;
        private readonly IUserRepository _userRepository;
        private readonly IQuery<List<User>, GetAllCriteria> _getAllUsersQuery;
        private readonly IQuery<Player, FindByIdCriteria> _getUserPlayerQuery;
        private readonly ICacheProvider _cacheProvider;
        private readonly IQuery<List<User>, UniqueUserCriteria> _getAdminsListQuery;

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
        public UserService(
            IAuthorizationService authService,
            IQuery<User, FindByIdCriteria> getUserByIdQuery,
            IQuery<List<User>, GetAllCriteria> getAllUsersQuery,
            IQuery<Player, FindByIdCriteria> getUserPlayerQuery,
            ICacheProvider cacheProvider,
            IQuery<List<User>, UniqueUserCriteria> getAdminsListQuery,
            IUserRepository userRepository)
        {
            _authService = authService;
            _getUserByIdQuery = getUserByIdQuery;
            _getAllUsersQuery = getAllUsersQuery;
            _getUserPlayerQuery = getUserPlayerQuery;
            _cacheProvider = cacheProvider;
            _getAdminsListQuery = getAdminsListQuery;
            _userRepository = userRepository;
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
        /// Gets User entity by Id.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <returns>User entity.</returns>
        public User GetUserDetails(int userId)
        {
            this._authService.CheckAccess(AuthOperations.AllUsers.ViewDetails);
            var user = this.GetUser(userId);
            if (user != null)
            {
                user.Player = this.GetPlayer(user.PlayerId.GetValueOrDefault());
            }

            return user;
        }

        /// <summary>
        /// Get all users collection.
        /// </summary>
        /// <returns>Use collection.</returns>
        public List<User> GetAllUsers()
        {
            this._authService.CheckAccess(AuthOperations.AllUsers.ViewList);
            return this._getAllUsersQuery.Execute(new GetAllCriteria());
        }

        /// <summary>
        /// Get all users collection.
        /// </summary>
        /// <returns>Use collection.</returns>
        public List<User> GetAllActiveUsers()
        {
            this._authService.CheckAccess(AuthOperations.AllUsers.ViewActiveList);
            var activeUsersList = _cacheProvider["ActiveUsers"] as List<int> ?? new List<int>();
            _cacheProvider["ActiveUsers"] = activeUsersList;
            return activeUsersList.Select(GetUser).ToList();
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

        /// <summary>
        /// Get player instance by Id.
        /// </summary>
        /// <param name="playerId">Player Id.</param>
        /// <returns>Player instance.</returns>
        private Player GetPlayer(int playerId)
        {
            return this._getUserPlayerQuery.Execute(new FindByIdCriteria { Id = playerId });
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