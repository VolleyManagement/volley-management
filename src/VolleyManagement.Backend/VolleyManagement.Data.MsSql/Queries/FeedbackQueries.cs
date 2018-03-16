﻿namespace VolleyManagement.Data.MsSql.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Contracts;
    using Data.Queries.Common;
    using Domain.FeedbackAggregate;
    using Entities;

    /// <summary>
    /// Provides Object Query implementation for Roles
    /// </summary>
    public class FeedbackQueries : IQuery<ICollection<Feedback>, GetAllCriteria>,
                                    IQuery<Feedback, FindByIdCriteria>
    {
        #region Fields

        private readonly VolleyUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackQueries"/> class.
        /// </summary>
        /// <param name="unitOfWork"> The unit of work. </param>
        public FeedbackQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
        }

        #endregion

        #region Implemenations

        /// <summary>
        /// Finds Feedback by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Feedback"/>. </returns>
        public ICollection<Feedback> Execute(GetAllCriteria criteria)
        {
            return _unitOfWork.Context.Feedbacks.Select(GetFeedbackMapping()).ToList();
        }

        /// <summary>
        /// Finds Feedbacks by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Feedback"/>. </returns>
        public Feedback Execute(FindByIdCriteria criteria)
        {
            return _unitOfWork.Context.Feedbacks
                                      .Where(r => r.Id == criteria.Id)
                                      .Select(GetFeedbackMapping())
                                      .SingleOrDefault();
        }

        #endregion

        #region Mapping

        private static Expression<Func<FeedbackEntity, Feedback>> GetFeedbackMapping()
        {
            return
                t =>
                new Feedback
                {
                    Id = t.Id,
                    UsersEmail = t.UsersEmail,
                    Content = t.Content,
                    Status = (FeedbackStatusEnum)t.Status,
                    Date = t.Date,
                    AdminName = t.AdminName,
                    UpdateDate = t.UpdateDate,
                    UserEnvironment = t.UserEnvironment
                };
        }

        #endregion
    }
}