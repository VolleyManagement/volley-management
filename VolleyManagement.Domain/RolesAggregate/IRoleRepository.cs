namespace VolleyManagement.Domain.RolesAggregate
{
    using Data.Contracts;

    /// <summary>
    /// Represents management operations over Role entity
    /// </summary>
    public interface IRoleRepository : IRepository
    {
        /// <summary>
        /// Adds user to role
        /// </summary>
        /// <param name="roleId">Role to add</param>
        /// <param name="userId">User to assign role</param>
        void AddUserToRole(int roleId, int userId);

        /// <summary>
        /// Remove user from role
        /// </summary>
        /// <param name="roleId">Role to add</param>
        /// <param name="userId">User to assign role</param>
        void RemoveUserFromRole(int roleId, int userId);
    }
}