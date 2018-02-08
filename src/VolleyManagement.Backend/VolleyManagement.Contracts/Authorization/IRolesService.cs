namespace VolleyManagement.Contracts.Authorization
{
    using System.Collections.Generic;

    using Domain.Dto;
    using Domain.RolesAggregate;
    using Domain.UsersAggregate;

    /// <summary>
    /// Provides Roles management
    /// </summary>
    public interface IRolesService
    {
        /// <summary>
        /// Returns all roles supported by the application
        /// </summary>
        /// <returns>List of roles</returns>
        List<Role> GetAllRoles();

        /// <summary>
        /// Retrieves role by specified Id
        /// </summary>
        /// <param name="roleId">Id of the role to look for</param>
        /// <returns>Role instance</returns>
        Role GetRole(int roleId);

        /// <summary>
        /// Retrieves role by specified Id
        /// </summary>
        /// <param name="roleId">Id of the role to look for</param>
        /// <returns>User in role</returns>
        List<UserInRoleDto> GetUsersInRole(int roleId);

        /// <summary>
        /// The get all users with roles.
        /// </summary>
        /// <returns> Collection of <see cref="UserInRoleDto"/></returns>
        List<UserInRoleDto> GetAllUsersWithRoles();

        /// <summary>
        /// The change role membership.
        /// </summary>
        /// <param name="roleId"> The role id. </param>
        /// <param name="usersToAdd"> The users to add. </param>
        /// <param name="usersToDelete"> The users to delete. </param>
        void ChangeRoleMembership(int roleId, int[] usersToAdd, int[] usersToDelete);
    }
}