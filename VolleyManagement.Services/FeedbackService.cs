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
    using Crosscutting.MailService;
    using Domain.Dto;
    using Domain.FeedbackAggregate;

    /// <summary>
    /// Represents an implementation of IFeedbackService contract.
    /// </summary>
    public class FeedbackService : IFeedbackService
    {
        #region Fields

        private readonly IFeedbackRepository _feedbackRepository;

        private readonly IMailService _mailService;

        private readonly IRolesService _roleService;

        private readonly IVolleyUserStore _volleyUserStore;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackService"/> class.
        /// </summary>
        /// <param name="feedbackRepository"> Read the IFeedbackRepository instance</param>
        /// <param name="mailService">Instance of the class
        /// that implements <see cref="IMailService"/></param>
        /// <param name="rolesService">Roles service.</param>
        /// <param name="volleyUserStore">Volley User Store.</param>
        public FeedbackService(
            IFeedbackRepository feedbackRepository,
            IMailService mailService,
            IRolesService rolesService,
            IVolleyUserStore volleyUserStore)
        {
            _feedbackRepository = feedbackRepository;
            _mailService = mailService;
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

            EmailMessage emailMessage = new EmailMessage(GetSenderEmailAddress(), emailTo, subject, body);
            _mailService.Send(emailMessage);
        }

        /// <summary>
        /// Send a feedback email to all admins.
        /// </summary>
        /// <param name="feedback">Feedback to send.</param>
        private void NotifyAdmins(Feedback feedback)
        {
            const int ADMIN_ROLE_ID = 1;

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

            List<UserInRoleDto> adminsList = _roleService.GetUsersInRole(ADMIN_ROLE_ID);

            foreach (var admin in adminsList)
            {
                var usersTask = Task.Run(() => _volleyUserStore.FindByIdAsync(admin.UserId));
                var user = usersTask.Result;

                EmailMessage emailMessage = new EmailMessage(GetSenderEmailAddress(), user.Email, subject, body);
                _mailService.Send(emailMessage);
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