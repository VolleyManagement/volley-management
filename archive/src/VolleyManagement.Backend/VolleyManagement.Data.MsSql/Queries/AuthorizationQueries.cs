﻿namespace VolleyManagement.Data.MsSql.Queries
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Contracts;
    using Data.Queries.Common;
    using Domain.RolesAggregate;
    using Entities;

    /// <summary>
    /// Provides query object implementation for authorization
    /// </summary>
    public class AuthorizationQueries : IQuery<ICollection<AuthOperation>, FindByUserIdCriteria>
    {
        private readonly DbSet<UserEntity> _dalUsers;
        private readonly DbSet<RoleToOperationEntity> _dalRolesToOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationQueries"/> class
        /// </summary>
        /// <param name="unitOfWork">Instance of class which implements <see cref="IUnitOfWork"/></param>
        public AuthorizationQueries(IUnitOfWork unitOfWork)
        {
            var unitoOfWork = (VolleyUnitOfWork)unitOfWork;
            _dalUsers = unitoOfWork.Context.Users;
            _dalRolesToOperations = unitoOfWork.Context.RolesToOperations;
        }


        /// <summary>
        /// Finds list of allowed operation by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The list of<see cref="AuthOperation"/>. </returns>
        public ICollection<AuthOperation> Execute(FindByUserIdCriteria criteria)
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
