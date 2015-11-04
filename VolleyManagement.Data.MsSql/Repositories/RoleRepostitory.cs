namespace VolleyManagement.Data.MsSql.Repositories
{
    using System.Data.Entity;

    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Domain.RolesAggregate;

    /// <summary>
    /// Defines implementation of the IUserRepository contract.
    /// </summary>
    internal class RoleRepostitory : IRoleRepository
    {
        /// <summary>
        /// Holds object set of DAL roles.
        /// </summary>
        private readonly DbSet<RoleEntity> _dalRoles;

        /// <summary>
        /// Holds object set of DAL users.
        /// </summary>
        private readonly DbSet<UserEntity> _dalUsers;

        /// <summary>
        /// Holds UnitOfWork instance.
        /// </summary>
        private readonly VolleyUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRepostitory"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public RoleRepostitory(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
            _dalRoles = _unitOfWork.Context.Roles;
            _dalUsers = _unitOfWork.Context.Users;
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return this._unitOfWork; }
        }

        /// <summary>
        /// Adds user to role
        /// </summary>
        /// <param name="roleId">Role to add</param>
        /// <param name="userId">User to assign role</param>
        public void AddUserToRole(int roleId, int userId)
        {
            var roleEntity = _dalRoles.Find(roleId);
            var userEntity = _dalUsers.Find(userId);

            roleEntity.Users.Add(userEntity);
        }

        /// <summary>
        /// Remove user from role
        /// </summary>
        /// <param name="roleId">Role to add</param>
        /// <param name="userId">User to assign role</param>
        public void RemoveUserFromRole(int roleId, int userId)
        {
            var roleEntity = _unitOfWork.Context.Roles.Find(roleId);
            var userEntity = _unitOfWork.Context.Users.Find(userId);

            roleEntity.Users.Remove(userEntity);
        }
    }
}