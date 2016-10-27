namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using System.Web.Configuration;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authentication;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Domain.Dto;
    using VolleyManagement.Domain.FeedbackAggregate;

    /// <summary>
    /// Defines MailService.
    /// </summary>
    public class MailService : IMailService
    {
        private readonly IRolesService _roleService;

        private readonly IVolleyUserStore _volleyUserStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailService"/> class.
        /// </summary>
        /// <param name="rolesService">Roles service.</param>
        /// /// <param name="volleyUserStore">Volley User Store.</param>
        public MailService(
            IRolesService rolesService,
            IVolleyUserStore volleyUserStore)
        {
            _roleService = rolesService;
            _volleyUserStore = volleyUserStore;
        }

        /// <summary>
        /// Send a confirmation email to user.
        /// </summary>
        /// <param name="emailTo">Recipient email.</param>
        public void NotifyUser(string emailTo)
        {
            string body = Properties.Resources.FeedbackConfirmationLetterBody;
            string subject = Properties.Resources.FeedbackConfirmationLetterSubject;

            Send(GetSenderEmailAddress(), GetSenderPassword(), body, subject, emailTo);
        }

        /// <summary>
        /// Send a feedback email to all admins.
        /// </summary>
        /// <param name="feedback">Feedback to send.</param>
        public void NotifyAdmins(Feedback feedback)
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

                Send(GetSenderEmailAddress(), GetSenderPassword(), body, subject, user.Email);
            }
        }

        /// <summary>
        /// Send an email.
        /// </summary>
        /// <param name="emailFrom">Sender email address.</param>
        /// <param name="password">Password for sender's email address.</param>
        /// <param name="body">Body of the email.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="emailsTo">Array of recipients' email addresses.</param>
        public void Send(string emailFrom, string password, string body, string subject, params string[] emailsTo)
        {
            if (string.IsNullOrEmpty(emailFrom)
                || string.IsNullOrEmpty(password)
                || string.IsNullOrEmpty(body)
                || string.IsNullOrEmpty(subject)
                || emailsTo == null)
            {
                throw new ArgumentException();
            }

            SmtpClient smtp = GetSmtpClient(emailFrom, password);

            MailMessage message = new MailMessage()
            {
                From = new MailAddress(emailFrom),
                Priority = MailPriority.High,
                Subject = subject,
                Body = body
            };

            foreach (var emailTo in emailsTo)
            {
                message.To.Add(emailTo);
            }

            smtp.Send(message);
        }

        /// <summary>
        /// Send an email.
        /// </summary>
        /// <param name="message">Message to send.</param>
        public void Send(MailMessage message)
        {
            if (message == null)
            {
                throw new ArgumentException();
            }

            SmtpClient smtp = GetSmtpClient(GetSenderEmailAddress(), GetSenderPassword());

            smtp.Send(message);
        }

        /// <summary>
        /// Gets sender email address for authorization.
        /// </summary>
        /// <returns>Email address.</returns>
        private string GetSenderEmailAddress()
        {
            const string EMAIL_ADDRESS_KEY = "GoogleEmailAddress";

            var emailAddress = WebConfigurationManager.AppSettings[EMAIL_ADDRESS_KEY];

            if (emailAddress == null)
            {
                throw new ArgumentNullException();
            }

            return emailAddress;
        }

        /// <summary>
        /// Gets sender email password for authorization.
        /// </summary>
        /// <returns>Email password.</returns>
        private string GetSenderPassword()
        {
            const string EMAIL_PASSWORD_KEY = "GoogleEmailPassword";

            var password = WebConfigurationManager.AppSettings[EMAIL_PASSWORD_KEY];

            if (password == null)
            {
                throw new ArgumentNullException();
            }

            return password;
        }

        /// <summary>
        /// Returns Simple Mail Transfer Protocol.
        /// </summary>
        /// <param name="email">Email that is being used.</param>
        /// <param name="password">Password to the email.</param>
        /// <returns>Valid protocol for sending emails.</returns>
        private SmtpClient GetSmtpClient(string email, string password)
        {
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(email, password)
            };

            return smtp;
        }
    }
}
