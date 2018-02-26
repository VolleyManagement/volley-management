namespace VolleyManagement.Data.MsSql.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Contracts;
    using Data.Queries.Common;
    using Domain.RolesAggregate;
    using Entities;

    /// <summary>
    /// Provides Object Query implementation for Roles
    /// </summary>
    public class RolesQueries : IQuery<ICollection<Role>, GetAllCriteria>,
                                IQuery<Role, FindByIdCriteria>
    {
        #region Fields

        private readonly VolleyUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RolesQueries"/> class.
        /// </summary>
        /// <param name="unitOfWork"> The unit of work. </param>
        public RolesQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
        }

        #endregion

        #region Implemenations

        /// <summary>
        /// Finds Roles by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Role"/>. </returns>
        public ICollection<Role> Execute(GetAllCriteria criteria)
        {
            return _unitOfWork.Context.Roles.Select(GetRoleMapping()).ToList();
        }

        /// <summary>
        /// Finds Roles by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Role"/>. </returns>
        public Role Execute(FindByIdCriteria criteria)
        {
            return _unitOfWork.Context.Roles
                                      .Where(r => r.Id == criteria.Id)
                                      .Select(GetRoleMapping())
                                      .SingleOrDefault();
        }

        #endregion

        #region Mapping

        private static Expression<Func<RoleEntity, Role>> GetRoleMapping()
        {
            return
                t =>
                new Role
                {
                    Id = t.Id,
                    Name = t.Name
                };
        }

        #endregion
    }
}