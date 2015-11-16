namespace VolleyManagement.Data.MsSql.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.Division;
    using VolleyManagement.Domain.DivisionAggregate;

    /// <summary>
    /// Provides Query Object implementation for Division entity
    /// </summary>
    public class DivisionQueries : IQuery<Division, FindByIdCriteria>,
                                   IQuery<List<Division>, TournamentDivisionsCriteria>
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
        /// Finds division by given criteria
        /// </summary>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Specific division</returns>
        public Division Execute(FindByIdCriteria criteria)
        {
            return _unitOfWork.Context.Divisions
                                      .Where(d => d.Id == criteria.Id)
                                      .Select(GetDivisionMapping())
                                      .SingleOrDefault();
        }

        /// <summary>
        /// Gets all Divisions of specific tournament
        /// </summary>
        /// <param name="criteria">tournament criteria</param>
        /// <returns>List of divisions</returns>
        public List<Division> Execute(TournamentDivisionsCriteria criteria)
        {
            return _unitOfWork.Context.Divisions
                                      .Where(d => d.TournamentId == criteria.TournamentId)
                                      .Select(GetDivisionMapping())
                                      .ToList();
        }

        #region Mapping

        private static Expression<Func<DivisionEntity, Division>> GetDivisionMapping()
        {
            return d => new Division
            {
                Id = d.Id,
                Name = d.Name,
                TournamentId = d.TournamentId
            };
        }

        #endregion

        #endregion
    }
}
