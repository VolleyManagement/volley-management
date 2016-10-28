namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Configuration;
    using Contracts;
    using Contracts.Authentication;
    using Contracts.Authorization;
    using Crosscutting.Contracts.Providers;
    using Domain;
    using Domain.Dto;
    using Domain.FeedbackAggregate;

    /// <summary>
    /// Represents an implementation of IFeedbackService contract.
    /// </summary>
    public class FeedbackService : IFeedbackService
    {
        #region Fields

        private readonly IFeedbackRepository _feedbackRepository;

        /// <summary>
        /// Holds GmailAccountMailService instance.
        /// </summary>
        private readonly IGmailAccountMailService _gmailAccountMailService;

        private readonly IRolesService _roleService;

        private readonly IVolleyUserStore _volleyUserStore;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackService"/> class.
        /// </summary>
        /// <param name="feedbackRepository"> Read the IFeedbackRepository instance</param>
        /// <param name="gmailAccountMailService">Instance of the class
        /// that implements <see cref="IGmailAccountMailService"/></param>
        /// <param name="rolesService">Roles service.</param>
        /// <param name="volleyUserStore">Volley User Store.</param>
        public FeedbackService(
            IFeedbackRepository feedbackRepository,
            IGmailAccountMailService gmailAccountMailService,
            IRolesService rolesService,
            IVolleyUserStore volleyUserStore)
        {
            _feedbackRepository = feedbackRepository;
            _gmailAccountMailService = gmailAccountMailService;
            _roleService = rolesService;
            _volleyUserStore = volleyUserStore;
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

            NotifyUser(feedbackToCreate.UsersEmail);
            NotifyAdmins(feedbackToCreate);
        }

        #endregion

        #region Privates

        private void UpdateFeedbackDate(Feedback feedbackToUpdate)
        {
            feedbackToUpdate.Date = TimeProvider.Current.UtcNow;
        }

        /// <summary>
        /// Send a confirmation email to user.
        /// </summary>
        /// <param name="emailTo">Recipient email.</param>
        private void NotifyUser(string emailTo)
        {
            string body = Properties.Resources.FeedbackConfirmationLetterBody;
            string subject = Properties.Resources.FeedbackConfirmationLetterSubject;

            GmailMessage gmailMessage = new GmailMessage(GetSenderEmailAddress(), emailTo, subject, body);
            _gmailAccountMailService.Send(gmailMessage);
        }

        /// <summary>
        /// Send a feedback email to all admins.
        /// </summary>
        /// <param name="feedback">Feedback to send.</param>
        private void NotifyAdmins(Feedback feedback)
        {
            const int ROLE_ID = 1;

            string subject = string.Format(
                Properties.Resources.FeedbackConfirmationLetterSubject,
                feedback.Id);

            string body = string.Format(
                Properties.Resources.FeedbackEmailBodyToAdmins,
                feedback.Id,
                feedback.Date,
                feedback.UsersEmail,
                feedback.Status,
                feedback.Content);

            List<UserInRoleDto> adminsList = _roleService.GetUsersInRole(ROLE_ID);

            foreach (var admin in adminsList)
            {
                var usersTask = Task.Run(() => _volleyUserStore.FindByIdAsync(admin.UserId));
                var user = usersTask.Result;

                GmailMessage gmailMessage = new GmailMessage(GetSenderEmailAddress(), user.Email, subject, body);
                _gmailAccountMailService.Send(gmailMessage);
            }
        }

        private string GetSenderEmailAddress()
        {
            const string EMAIL_ADDRESS_KEY = "GoogleEmailAddress";

            var emailAddress = WebConfigurationManager.AppSettings[EMAIL_ADDRESS_KEY];

            if (emailAddress == null)
            {
                throw new ArgumentNullException(
                    Properties.Resources.ArgumentNullExceptionInvalidGmailAddress,
                    Properties.Resources.GmailAddress);
            }

            return emailAddress;
        }

        #endregion
    }
}