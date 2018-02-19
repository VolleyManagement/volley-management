namespace VolleyManagement.Services.Authorization
{
    using System.Collections.Generic;

    using Contracts.Authorization;
    using Data.Contracts;
    using Data.Queries.Common;
    using Data.Queries.User;
    using Domain.Dto;
    using Domain.RolesAggregate;

    /// <summary>
    /// Manages roles in the application
    /// </summary>
    public class RolesService : IRolesService
    {
        #region Fields

        private readonly IQuery<List<Role>, GetAllCriteria> _getAllQuery;

        private readonly IQuery<Role, FindByIdCriteria> _getByIdQuery;

        private readonly IQuery<List<UserInRoleDto>, FindByRoleCriteria> _getUsersByRoleQuery;

        private readonly IQuery<List<UserInRoleDto>, GetAllCriteria> _getUserInRolesQuery;

        private readonly IRoleRepository _roleRepository;

        #endregion

        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="RolesService"/> class.
        /// </summary>
        /// <param name="getAllQuery"> The get all query. </param>
        /// <param name="getByIdQuery"> The get by Id query. </param>
        /// <param name="getUsersByRoleQuery"> Users By Role query. </param>
        /// <param name="getUserInRolesQuery"> Users In Role query. </param>
        /// <param name="roleRepository">Role repository</param>
        public RolesService(
            IQuery<List<Role>, GetAllCriteria> getAllQuery,
            IQuery<Role, FindByIdCriteria> getByIdQuery,
            IQuery<List<UserInRoleDto>, FindByRoleCriteria> getUsersByRoleQuery,
            IQuery<List<UserInRoleDto>, GetAllCriteria> getUserInRolesQuery,
            IRoleRepository roleRepository)
        {
            _getAllQuery = getAllQuery;
            _getByIdQuery = getByIdQuery;
            _getUsersByRoleQuery = getUsersByRoleQuery;
            _getUserInRolesQuery = getUserInRolesQuery;
            _roleRepository = roleRepository;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Returns all roles supported by the application
        /// </summary>
        /// <returns>List of roles</returns>
        public ICollection<Role> GetAllRoles()
        {
            return _getAllQuery.Execute(new GetAllCriteria());
        }

        /// <summary>
        /// Retrieves role by specified Id
        /// </summary>
        /// <param name="roleId">Id of the role to look for</param>
        /// <returns>Role instance</returns>
        public Role GetRole(int roleId)
        {
            return _getByIdQuery.Execute(new FindByIdCriteria { Id = roleId });
        }

        /// <summary>
        /// Retrieves role by specified Id
        /// </summary>
        /// <param name="roleId">Id of the role to look for</param>
        /// <returns>User in role</returns>
        public ICollection<UserInRoleDto> GetUsersInRole(int roleId)
        {
            return _getUsersByRoleQuery.Execute(new FindByRoleCriteria { RoleId = roleId });
        }

        /// <summary>
        /// The get all users with roles.
        /// </summary>
        /// <returns> Collection of <see cref="UserInRoleDto"/></returns>
        public ICollection<UserInRoleDto> GetAllUsersWithRoles()
        {
            return _getUserInRolesQuery.Execute(new GetAllCriteria());
        }

        /// <summary>
        /// The change role membership.
        /// </summary>
        /// <param name="roleId"> The role id. </param>
        /// <param name="usersToAdd"> The users to add. </param>
        /// <param name="usersToDelete"> The users to delete. </param>
        public void ChangeRoleMembership(int roleId, int[] usersToAdd, int[] usersToDelete)
        {
            foreach (var userId in usersToAdd)
            {
                _roleRepository.AddUserToRole(roleId, userId);
            }

            foreach (var userId in usersToDelete)
            {
                _roleRepository.RemoveUserFromRole(roleId, userId);
            }

            if (usersToAdd.Length + usersToDelete.Length > 0)
            {
                _roleRepository.UnitOfWork.Commit();
            }
        }

        #endregion
    }
}