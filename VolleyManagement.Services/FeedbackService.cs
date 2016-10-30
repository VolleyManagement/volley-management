namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using Contracts;
    using Crosscutting.Contracts.Providers;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Domain.FeedbackAggregate;
    using VolleyManagement.Domain.RolesAggregate;
    using VolleyManagement.Domain.UsersAggregate;

    /// <summary>
    /// Represents an implementation of IFeedbackService contract.
    /// </summary>
    public class FeedbackService : IFeedbackService
    {
        #region Fields

        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuthorizationService _authService;
        private readonly IQuery<Feedback, FindByIdCriteria> _getFeedbackByIdQuery;
        private readonly IQuery<List<Feedback>, GetAllCriteria> _getAllFeedbacksQuery;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackService"/> class.
        /// </summary>
        /// <param name="feedbackRepository"> Read the IFeedbackRepository instance</param>
        /// <param name="userService">Instance of class which implements <see cref="IUserService"/></param>
        /// <param name="currentUserService">Instance of the class </param>
        /// <param name="authService">Instance of class which implements <see cref="IAuthorizationService"/></param>
        /// <param name="getFeedbackByIdQuery">Get feedback by it's id </param>
        /// <param name="getAllFeedbacksQuery">Get list of all feedbacks</param>
        public FeedbackService(
            IFeedbackRepository feedbackRepository,
            IUserService userService,
            ICurrentUserService currentUserService,
            IAuthorizationService authService,
            IQuery<Feedback, FindByIdCriteria> getFeedbackByIdQuery,
            IQuery<List<Feedback>, GetAllCriteria> getAllFeedbacksQuery)
        {
            _feedbackRepository = feedbackRepository;
            _userService = userService;
            _authService = authService;
            _currentUserService = currentUserService;
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
            _authService.CheckAccess(AuthOperations.Feedbacks.Read);
            var feedback = Get(id);

            if (feedback == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.FeedbackNotFound);
            }

            ChangeFeedbackStatus(feedback, FeedbackStatusEnum.Read);

            return feedback;
        }

        /// <summary>
        /// Close a Feedback.
        /// </summary>
        /// <param name="id">id for close.</param>
        public void Close(int id)
        {
            _authService.CheckAccess(AuthOperations.Feedbacks.Close);
            var feedback = Get(id);

            if (feedback == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.FeedbackNotFound);
            }

            ChangeFeedbackStatus(feedback, FeedbackStatusEnum.Closed);
        }

        /// <summary>
        /// Reply the answer to user.
        /// </summary>
        /// <param name="id">id for reply.</param>
        public void Reply(int id)
        {
            _authService.CheckAccess(AuthOperations.Feedbacks.Reply);

            var feedback = Get(id);

            if (feedback == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.FeedbackNotFound);
            }

            ChangeFeedbackStatus(feedback, FeedbackStatusEnum.Answered);
        }

        /// <summary>
        /// Change status of the feedback
        /// </summary>
        /// <param name="feedback">id for reply.</param>
        /// <param name="newStatusCode">Information about mail (body, receiver)</param>
        private void ChangeFeedbackStatus(Feedback feedback, FeedbackStatusEnum newStatusCode)
        {
                feedback.Status = newStatusCode;
                if (ShouldChangeLastUpdateInfo(newStatusCode))
                {
                    int userId = this._currentUserService.GetCurrentUserId();
                    User user = this._userService.GetUser(userId);
                    feedback.UpdateDate = TimeProvider.Current.UtcNow;
                    feedback.AdminName = user.PersonName;
                }

                _feedbackRepository.Update(feedback);
                _feedbackRepository.UnitOfWork.Commit();
        }

        #endregion

        #region Privates

        private bool ShouldChangeLastUpdateInfo(FeedbackStatusEnum newStatusCode)
        {
            return newStatusCode == FeedbackStatusEnum.Closed || newStatusCode == FeedbackStatusEnum.Answered;
        }

        private void UpdateFeedbackDate(Feedback feedbackToUpdate)
        {
            feedbackToUpdate.Date = TimeProvider.Current.UtcNow;
        }

        #endregion
    }
}