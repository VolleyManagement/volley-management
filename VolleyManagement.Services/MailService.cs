namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using System.Web.Configuration;
    using Authentication;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Domain.Dto;
    using VolleyManagement.Domain.FeedbackAggregate;

    /// <summary>
    /// Defines MailService.
    /// </summary>
    public class MailService : IMailService
    {
        private readonly IRolesService _roleService;

        private readonly VolleyUserStore _volleyUserStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailService"/> class.
        /// </summary>
        /// <param name="rolesService">Roles service.</param>
        /// /// <param name="volleyUserStore">Volley User Store.</param>
        public MailService(
            IRolesService rolesService,
            VolleyUserStore volleyUserStore)
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
            // достаем логин и пароль из других сервисов?
            const string EMAIL_ADDRESS_KEY = "GoogleEmailAddress";
            const string EMAIL_PASSWORD_KEY = "GoogleEmailPassword";

            var emailFrom = WebConfigurationManager.AppSettings[EMAIL_ADDRESS_KEY];
            var password = WebConfigurationManager.AppSettings[EMAIL_PASSWORD_KEY];

            if (emailFrom == null || password == null)
            {
                throw new ArgumentNullException();
            }

            string body = Properties.Resources.FeedbackConfirmationLetterBody;
            string subject = Properties.Resources.FeedbackConfirmationLetterSubject;

            Send(emailFrom, password, body, subject, emailTo);
        }

        /// <summary>
        /// Send a feedback email to all admins.
        /// </summary>
        /// <param name="feedback">Feedback to send.</param>
        public void NotifyAdmins(Feedback feedback)
        {
            // достаем логин-пароль из другого сервиса?
            const string EMAIL_ADDRESS_KEY = "GoogleEmailAddress";
            const string EMAIL_PASSWORD_KEY = "GoogleEmailPassword";
            const int ROLE_ID = 1;

            var emailFrom = WebConfigurationManager.AppSettings[EMAIL_ADDRESS_KEY];
            var password = WebConfigurationManager.AppSettings[EMAIL_PASSWORD_KEY];

            if (emailFrom == null || password == null)
            {
                throw new ArgumentNullException();
            }

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

                Send(emailFrom, password, body, subject, user.Email);
            }
        }

        /// <summary>
        /// Send an email.
        /// </summary>
        /// <param name="emailTo">Recipient email.</param>
        /// <param name="body">Body of the email.</param>
        public void Send(string emailTo, string body)
        {
            const string EMAIL_ADDRESS_KEY = "GoogleEmailAddress";
            const string EMAIL_PASSWORD_KEY = "GoogleEmailPassword";

            var googleEmail = WebConfigurationManager.AppSettings[EMAIL_ADDRESS_KEY];
            var googleAddress = WebConfigurationManager.AppSettings[EMAIL_PASSWORD_KEY];

            if (googleEmail == null || googleAddress == null)
            {
                throw new ArgumentNullException();
            }

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(googleEmail, googleAddress)
            };

            MailMessage message = new MailMessage();
            message.From = new MailAddress(googleEmail);
            message.Priority = MailPriority.High;
            message.To.Add(emailTo);
            message.Subject = "Feedback notification";
            message.Body = body;
            smtp.Send(message);
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

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailFrom, password)
            };

            MailMessage message = new MailMessage();
            message.From = new MailAddress(emailFrom);
            message.Priority = MailPriority.High;
            foreach (var emailTo in emailsTo)
            {
                message.To.Add(emailTo);
            }

            message.Subject = subject;
            message.Body = body;
            smtp.Send(message);
        }

        public void Send(string emailTo, string emailFrom, string body)
        {
            throw new NotImplementedException();
        }
    }
}
