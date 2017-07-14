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
    /// Provides Object Query implementation for Divisions
    /// </summary>
    public class DivisionQueries : IQuery<List<Division>, GetAllCriteria>,
                                   IQuery<Division, FindByIdCriteria>
    {
        #region Fields

        private readonly VolleyUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DivisionQueries"/> class.
        /// </summary>
        /// <param name="unitOfWork"> The unit of work. </param>
        public DivisionQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
        }

        #endregion

        #region Implemenations

        /// <summary>
        /// Finds Division by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Division"/>. </returns>
        public List<Division> Execute(GetAllCriteria criteria)
        {
            return _unitOfWork.Context.Divisions.Select(GetDivisionMapping()).ToList();
        }

        /// <summary>
        /// Finds Division by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Division"/>. </returns>
        public Division Execute(FindByIdCriteria criteria)
        {
            return _unitOfWork.Context.Divisions
                                      .Where(t => t.Id == criteria.Id)
                                      .Select(GetDivisionMapping())
                                      .SingleOrDefault();
        }

        #endregion

        #region Mapping

        private static Expression<Func<DivisionEntity, Division>> GetDivisionMapping()
        {
            return
                d =>
                new Division
                {
                    Id = d.Id,
                    Name = d.Name,
                    TournamentId = d.TournamentId,
                    Groups = d.Groups
                                .AsQueryable()
                                .Where(g => g.DivisionId == d.Id)
                                .Select(GetGroupMapping())
                                .ToList()
                };
        }

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
