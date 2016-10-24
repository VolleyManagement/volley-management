namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Mail;
    using System.Web.Configuration;
    using Authentication;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Domain.Dto;
    using VolleyManagement.Domain.FeedbackAggregate;
    using Contracts.Authentication.Models;
    using System.Threading.Tasks;

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
        public MailService(IRolesService rolesService,
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
            Send(emailTo, "Thank you for your feedback!");
        }

        /// <summary>
        /// Send a feedback email to all admins.
        /// </summary>
        /// <param name="feedback">Feedback to send.</param>
        public void NotifyAdmins(Feedback feedback)
        {
            // формируем тело письма
            string emailBody = string.Format(
                Properties.Resources.FeedbackEmailBodyToAdmins,
                feedback.Id,
                feedback.Date,
                feedback.UsersEmail,
                feedback.Status,
                feedback.Content);

            // выбираем всех админов
            const int ROLE_ID = 1;
            List<UserInRoleDto> adminsList = _roleService.GetUsersInRole(ROLE_ID);

            // выбираем соответствующие емейлы
            foreach (var admin in adminsList)
            {
                var usersTask = Task.Run(() => _volleyUserStore.FindByIdAsync(admin.UserId));
                var user = usersTask.Result;

                Send(user.Email, emailBody);

            }
            // создаем список получателей
          //  Send(feedback.UsersEmail, emailBody);
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
        /// <param name="emailTo">Recipient email.</param>
        /// /// <param name="emailFrom">Sender email.</param>
        /// <param name="body">Body of the email.</param>
        public void Send(string emailTo, string emailFrom, string body)
        {
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailFrom, "password") // !!!
            };

            MailMessage message = new MailMessage();
            message.From = new MailAddress(emailFrom);
            message.Priority = MailPriority.High;
            message.To.Add(emailTo);
            message.Subject = "Notification from VolleyManagement"; // ??
            message.Body = body;
            smtp.Send(message);
        }
    }
}
