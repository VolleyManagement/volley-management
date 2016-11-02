using VolleyManagement.Contracts.Authorization;

namespace VolleyManagement.UI.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using Contracts;
    using Data.Contracts;
    using Data.Queries.Common;
    using Domain.UsersAggregate;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.RolesAggregate;

    /// <summary>
    /// Provides the way to get specified information about user.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IAuthorizationService _authService;
        private readonly IQuery<User, FindByIdCriteria> _getUserByIdQuery;
        private readonly IQuery<List<User>, GetAllCriteria> _getAllUsersQuery;
        private readonly IQuery<Player, FindByIdCriteria> _getUserPlayerQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="getUserByIdQuery">Query for getting User by Id.</param>
        /// <param name="getAllUsersQuery">Query for getting all User.</param>
        public UserService(
            IAuthorizationService authService,
            IQuery<User, FindByIdCriteria> getUserByIdQuery,
            IQuery<List<User>, GetAllCriteria> getAllUsersQuery,
            IQuery<Player,FindByIdCriteria> getUserPlayerQuery)
        {
            _authService = authService;
            _getUserByIdQuery = getUserByIdQuery;
            _getAllUsersQuery = getAllUsersQuery;
            _getUserPlayerQuery = getUserPlayerQuery;
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
            var user= this.GetUser(userId);
            user.Player = this.GetPlayer(user.PlayerId);

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
        /// Get player instance by Id.
        /// </summary>
        /// <param name="playerId">Player Id.</param>
        /// <returns>Player instance.</returns>
        private Player GetPlayer(int playerId)
        {
            return this._getUserPlayerQuery.Execute(new FindByIdCriteria { Id = playerId });
        }
    }
}