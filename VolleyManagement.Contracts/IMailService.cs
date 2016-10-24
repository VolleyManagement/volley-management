namespace VolleyManagement.Contracts
{
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
        /// Send an email with default email sender.
        /// </summary>
        /// <param name="emailTo">Recipient email.</param>
        /// <param name="body">Body of the email.</param>
        void Send(string emailTo, string body);

        /// <summary>
        /// Send an email.
        /// </summary>
        /// <param name="emailTo">Recipient email.</param>
        /// /// <param name="emailFrom">Sender email.</param>
        /// <param name="body">Body of the email.</param>
        void Send(string emailTo, string emailFrom, string body);
    }
}
