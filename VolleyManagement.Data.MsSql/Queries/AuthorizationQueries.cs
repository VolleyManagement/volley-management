namespace VolleyManagement.Data.MsSql.Queries
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Domain.RolesAggregate;

    /// <summary>
    /// Provides query object implementation for authorization
    /// </summary>
    public class AuthorizationQueries : IQuery<List<AuthOperation>, FindByUserIdCriteria>
    {
        private readonly VolleyUnitOfWork _unitOfWork;
        private readonly DbSet<UserEntity> _dalUsers;
        private readonly DbSet<RoleEntity> _dalRoles;
        private readonly DbSet<RoleToOperationEntity> _dalRolesToOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationQueries"/> class
        /// </summary>
        /// <param name="unitOfWork">Instance of class which implements <see cref="IUnitOfWork"/></param>
        public AuthorizationQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
            _dalUsers = _unitOfWork.Context.Users;
            _dalRoles = _unitOfWork.Context.Roles;
            _dalRolesToOperations = _unitOfWork.Context.RolesToOperations;
        }

        /// <summary>
        /// Finds list of allowed operation by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The list of<see cref="AuthOperation"/>. </returns>
        public List<AuthOperation> Execute(FindByUserIdCriteria criteria)
        {
            var data = _dalUsers.Where(u => u.Id == criteria.UserId)
                                .SelectMany(u => u.Roles)
                                .Join(
                                    _dalRolesToOperations,
                                    r => r.Id,
                                    o => o.RoleId,
                                    (r, o) => o.OperationId)
                                .Distinct()
                                .AsEnumerable()
                                .Select(o => (AuthOperation)o)
                                .ToList();

            return data;
        }
    }
}
