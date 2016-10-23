namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using Contracts;
    using Crosscutting.Contracts.Providers;
    using Domain.FeedbackAggregate;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Domain.MailsAggregate;
    using VolleyManagement.Domain.UsersAggregate;

    /// <summary>
    /// Represents an implementation of IFeedbackService contract.
    /// </summary>
    public class FeedbackService : IFeedbackService
    {
        #region Fields

        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;
        private readonly IQuery<Feedback, FindByIdCriteria> _getFeedbackByIdQuery;
        private readonly IQuery<List<Feedback>, GetAllCriteria> _getAllFeedbacksQuery;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackService"/> class.
        /// </summary>
        /// <param name="feedbackRepository"> Read the IFeedbackRepository instance</param>
        /// <param name="userService">Instance of class which implements <see cref="IUserService"/>e</param>
        /// <param name="mailService">Instance of class which implements <see cref="IMailService"/></param>
        /// <param name="getFeedbackByIdQuery">Get By ID query for feedbacks</param>
        /// <param name="getAllFeedbacksQuery">Get All feedbacks query</param>
        public FeedbackService(
            IFeedbackRepository feedbackRepository,
            IUserService userService,
            IMailService mailService,
            IQuery<Feedback, FindByIdCriteria> getFeedbackByIdQuery,
            IQuery<List<Feedback>, GetAllCriteria> getAllFeedbacksQuery)
        {
            _feedbackRepository = feedbackRepository;
            _userService = userService;
            _mailService = mailService;
            _getFeedbackByIdQuery = getFeedbackByIdQuery;
            _getAllFeedbacksQuery = getAllFeedbacksQuery;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Creates feedback.
        /// </summary>
        /// <param name="feedbackToCreate">Feedback to create.</param>
        public void Create(Feedback feedbackToCreate)
        {
            if (feedbackToCreate == null)
            {
                throw new ArgumentNullException("feedback");
            }

            UpdateFeedbackDate(feedbackToCreate);
            _feedbackRepository.Add(feedbackToCreate);
            _feedbackRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Method to get all feedbacks.
        /// </summary>
        /// <returns>All feedbacks.</returns>
        public List<Feedback> Get()
        {
            return _getAllFeedbacksQuery.Execute(new GetAllCriteria());
        }

        /// <summary>
        /// Finds a Feedback by id.
        /// </summary>
        /// <param name="id">id for search.</param>
        /// <returns>founded feedback.</returns>
        public Feedback Get(int id)
        {
            return _getFeedbackByIdQuery.Execute(new FindByIdCriteria { Id = id });
        }

        /// <summary>
        /// Get a Feedback's details by id.
        /// </summary>
        /// <param name="id">id for details.</param>
        /// <returns>founded feedback.</returns>
        public Feedback GetDetails(int id)
        {
            var feedback = Get(id);
            try
            {
                ChangeFeedbackStatus(feedback, FeedbackStatusEnum.Read);
            }
            catch (ConcurrencyException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.FeedbackNotFound, ex);
            }
            return feedback;
        }

        /// <summary>
        /// Close a Feedback.
        /// </summary>
        /// <param name="id">id for close.</param>
        public void Close(int id)
        {
            var feedback = Get(id);
            try
            {
                ChangeFeedbackStatus(feedback, FeedbackStatusEnum.Closed);
            }
            catch (ConcurrencyException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.FeedbackNotFound, ex);
            }
        }

        /// <summary>
        /// Reply the answer to user.
        /// </summary>
        /// <param name="id">id for reply.</param>
        /// <param name="answerToSend">Information about mail (body, receiver)</param>
        public void Reply(int id, Mail answerToSend)
        {
            var feedback = Get(id);

            try
            {
                _mailService.Send(answerToSend);
                ChangeFeedbackStatus(feedback, FeedbackStatusEnum.Answered);
            }
            catch (ConcurrencyException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.FeedbackNotFound, ex);
            }
            catch (AuthorizationException)
            {
                throw new AuthorizationException();
            }
        }

        private void ChangeFeedbackStatus(Feedback feedback, FeedbackStatusEnum statusCode)
        {
            if (feedback == null)
            {
                throw new ConcurrencyException();
            }

            if (feedback.Status != statusCode)
            {
                feedback.Status = statusCode;
                if (feedback.Status == FeedbackStatusEnum.Closed || feedback.Status == FeedbackStatusEnum.Answered)
                {
                    User user = this._userService.GetCurrentUserInstance();
                    if (user == null)
                    {
                        throw new AuthorizationException();
                    }
                    feedback.UpdateDate = TimeProvider.Current.UtcNow;
                    feedback.AdminName = user.PersonName;
                }

                _feedbackRepository.Update(feedback);
                _feedbackRepository.UnitOfWork.Commit();
            }
        }

        #endregion

        #region Privates

        private void UpdateFeedbackDate(Feedback feedbackToUpdate)
        {
            feedbackToUpdate.Date = TimeProvider.Current.UtcNow;
        }

        #endregion
    }
}