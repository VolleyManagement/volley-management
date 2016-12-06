namespace VolleyManagement.Data.MsSql.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.Player;
    using VolleyManagement.Domain.RequestsAggregate;

    /// <summary>
    /// Provides Object Query implementation for Requests
    /// </summary>
    public class RequestQueries : IQuery<List<Request>, GetAllCriteria>,
                                  IQuery<Request, FindByIdCriteria>,
                                  IQuery<Request, UserToPlayerCriteria>
    {
        #region Fields

        private readonly VolleyUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestQueries"/> class.
        /// </summary>
        /// <param name="unitOfWork"> The unit of work. </param>
        public RequestQueries(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = (VolleyUnitOfWork)unitOfWork;
        }

        #endregion

        #region Implemenations

        /// <summary>
        /// Finds Requests by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Request"/>. </returns>
        public List<Request> Execute(GetAllCriteria criteria)
        {
            return _unitOfWork.Context.Requests.Select(GetRequestMapping()).ToList();
        }

        /// <summary>
        /// Finds Requests by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Request"/>. </returns>
        public Request Execute(FindByIdCriteria criteria)
        {
            return _unitOfWork.Context.Requests
                                      .Where(r => r.Id == criteria.Id)
                                      .Select(GetRequestMapping())
                                      .SingleOrDefault();
        }

        /// <summary>
        /// Finds Requests by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria.</param>
        /// <returns> The <see cref="Request"/>.</returns>
        public Request Execute(UserToPlayerCriteria criteria)
        {
            return _unitOfWork.Context.Requests
                                      .Where(r => r.PlayerId == criteria.PlayerId)
                                      .Where(r => r.UserId == criteria.UserId)
                                      .Select(GetRequestMapping())
                                      .SingleOrDefault();
        }

    #endregion

    #region Mapping

    private static Expression<Func<RequestEntity, Request>> GetRequestMapping()
        {
            return
                t =>
                new Request()
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    PlayerId = t.PlayerId
                };
        }

        #endregion
    }
}
