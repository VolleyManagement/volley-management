namespace VolleyManagement.Data.MsSql.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Contracts;
    using Data.Queries.Common;
    using Domain.TournamentsAggregate;
    using Entities;

    /// <summary>
    /// Provides Object Query implementation for Group
    /// </summary>
    public class GroupQueries : IQuery<List<Group>, GetAllCriteria>,
                                   IQuery<Group, FindByIdCriteria>
    {
        #region Fields

        private readonly VolleyUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DivisionQueries"/> class.
        /// </summary>
        /// <param name="unitOfWork"> The unit of work. </param>
        public GroupQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
        }

        #endregion

        #region Implemenations

        /// <summary>
        /// Finds Group by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Group"/>. </returns>
        public List<Group> Execute(GetAllCriteria criteria)
        {
            return _unitOfWork.Context.Groups.Select(GetGroupMapping()).ToList();
        }

        /// <summary>
        /// Finds Group by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Group"/>. </returns>
        public Group Execute(FindByIdCriteria criteria)
        {
            return _unitOfWork.Context.Groups
                                      .Where(t => t.Id == criteria.Id)
                                      .Select(GetGroupMapping())
                                      .SingleOrDefault();
        }

        #endregion

        #region Mapping

        private static Expression<Func<GroupEntity, Group>> GetGroupMapping()
        {
            return
                g =>
                new Group
                {
                    Id = g.Id,
                    Name = g.Name,
                    DivisionId = g.DivisionId,
                };
        }

        #endregion
    }
}
