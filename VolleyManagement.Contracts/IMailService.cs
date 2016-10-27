namespace VolleyManagement.Contracts
{
    using System.Net.Mail;
    using VolleyManagement.Domain.FeedbackAggregate;

    /// <summary>
    /// Defines a contract for Mail service.
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Send a confirmation email to user.
        /// </summary>
        /// <param name="emailTo">Recipient email.</param>
        void NotifyUser(string emailTo);

        /// <summary>
        /// Send a feedback email to all admins.
        /// </summary>
        /// <param name="feedback">Feedback to send.</param>
        void NotifyAdmins(Feedback feedback);

        /// <summary>
        /// Send an email.
        /// </summary>
        /// <param name="emailFrom">Sender email address.</param>
        /// <param name="password">Password for sender's email address.</param>
        /// <param name="body">Body of the email.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="emailsTo">Array of recipients' email addresses.</param>
        void Send(string emailFrom, string password, string body, string subject, params string[] emailsTo);

        /// <summary>
        /// Send an email.
        /// </summary>
        /// <param name="message">Email to send.</param>
        void Send(MailMessage message);
    }
}
