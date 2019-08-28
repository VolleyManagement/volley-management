namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using Contracts;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Contracts.ExternalResources;
    using Crosscutting.Contracts.Providers;
    using Data.Contracts;
    using Data.Queries.Common;
    using Domain.FeedbackAggregate;
    using Domain.RolesAggregate;
    using Domain.UsersAggregate;

    /// <summary>
    /// Represents an implementation of IFeedbackService contract.
    /// </summary>
    public class FeedbackService : IFeedbackService
    {
        #region Fields

        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuthorizationService _authService;
        private readonly IQuery<Feedback, FindByIdCriteria> _getFeedbackByIdQuery;
        private readonly IQuery<ICollection<Feedback>, GetAllCriteria> _getAllFeedbacksQuery;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackService"/> class.
        /// </summary>
        /// <param name="feedbackRepository"> Read the IFeedbackRepository instance</param>
        /// <param name="userService">Instance of class which implements <see cref="IUserService"/></param>
        /// <param name="mailService">Instance of class which implements <see cref="IMailService"/></param>
        /// <param name="currentUserService">Instance of the class </param>
        /// <param name="authService">Instance of class which implements <see cref="IAuthorizationService"/></param>
        /// <param name="getFeedbackByIdQuery">Get feedback by it's id </param>
        /// <param name="getAllFeedbacksQuery">Get list of all feedbacks</param>
        public FeedbackService(
            IFeedbackRepository feedbackRepository,
            IUserService userService,
            IMailService mailService,
            ICurrentUserService currentUserService,
            IAuthorizationService authService,
            IQuery<Feedback, FindByIdCriteria> getFeedbackByIdQuery,
            IQuery<ICollection<Feedback>, GetAllCriteria> getAllFeedbacksQuery)
        {
            _feedbackRepository = feedbackRepository;
            _userService = userService;
            _mailService = mailService;
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
                throw new ArgumentNullException(nameof(feedbackToCreate));
            }

            UpdateFeedbackDate(feedbackToCreate);
            _feedbackRepository.Add(feedbackToCreate);
            _feedbackRepository.UnitOfWork.Commit();

            NotifyUser(feedbackToCreate.UsersEmail);
            NotifyAdmins(feedbackToCreate);
        }

        /// <summary>
        /// Method to get all feedbacks.
        /// </summary>
        /// <returns>All feedbacks.</returns>
        public ICollection<Feedback> Get()
        {
            _authService.CheckAccess(AuthOperations.Feedbacks.Read);
            return _getAllFeedbacksQuery.Execute(new GetAllCriteria());
        }

        /// <summary>
        /// Finds a Feedback by id.
        /// </summary>
        /// <param name="id">id for search.</param>
        /// <returns>founded feedback.</returns>
        public Feedback Get(int id)
        {
            _authService.CheckAccess(AuthOperations.Feedbacks.Read);
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

            if (feedback.Status == FeedbackStatusEnum.New)
            {
                ChangeFeedbackStatus(feedback, FeedbackStatusEnum.Read);
            }

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
        /// <param name="message">message for reply.</param>
        public void Reply(int id, string message)
        {
            _authService.CheckAccess(AuthOperations.Feedbacks.Reply);

            var feedback = Get(id);

            if (feedback == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.FeedbackNotFound);
            }

            ChangeFeedbackStatus(feedback, FeedbackStatusEnum.Answered);
            NotifyUser(feedback.UsersEmail, message);
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
                var userId = _currentUserService.GetCurrentUserId();
                var user = _userService.GetUser(userId);
                feedback.UpdateDate = TimeProvider.Current.UtcNow;
                feedback.AdminName = user.PersonName;
            }

            _feedbackRepository.Update(feedback);
            _feedbackRepository.UnitOfWork.Commit();
        }

        #endregion

        #region Privates

        /// <summary>
        /// Send a confirmation email to user.
        /// </summary>
        /// <param name="emailTo">Recipient email.</param>
        private void NotifyUser(string emailTo)
        {
            var body = Properties.Resources.FeedbackConfirmationLetterBody;
            var subject = Properties.Resources.FeedbackConfirmationLetterSubject;

            var emailMessage = new EmailMessage(emailTo, subject, body);
            _mailService.Send(emailMessage);
        }

        /// <summary>
        /// Send a confirmation email to user.
        /// </summary>
        /// <param name="emailTo">Recipient email.</param>
        /// <param name="message">Message for reply</param>
        private void NotifyUser(string emailTo, string message)
        {
            var subject = Properties.Resources.FeedbacksEmailReplySubject;
            var emailMessage = new EmailMessage(emailTo, subject, message);
            _mailService.Send(emailMessage);
        }

        /// <summary>
        /// Send a feedback email to all admins.
        /// </summary>
        /// <param name="feedback">Feedback to send.</param>
        private void NotifyAdmins(Feedback feedback)
        {
            var subject = string.Format(
                Properties.Resources.FeedbackEmailSubjectToAdmins,
                feedback.Id);

            var body = string.Format(
                Properties.Resources.FeedbackEmailBodyToAdmins,
                feedback.Id,
                feedback.Date,
                feedback.UsersEmail,
                feedback.Status,
                feedback.Content);

            var adminsList = _userService.GetAdminsList();

            foreach (var admin in adminsList)
            {
                var emailMessage = new EmailMessage(admin.Email, subject, body);
                _mailService.Send(emailMessage);
            }
        }

        private static bool ShouldChangeLastUpdateInfo(FeedbackStatusEnum newStatusCode)
        {
            return newStatusCode == FeedbackStatusEnum.Closed || newStatusCode == FeedbackStatusEnum.Answered;
        }

        private static void UpdateFeedbackDate(Feedback feedbackToUpdate)
        {
            feedbackToUpdate.Date = TimeProvider.Current.UtcNow;
        }
        #endregion
    }
}