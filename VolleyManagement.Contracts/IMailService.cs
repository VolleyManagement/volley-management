namespace VolleyManagement.Contracts
{
    using VolleyManagement.Domain.FeedbackAggregate;
    using VolleyManagement.Domain.MailsAggregate;

    /// <summary>
    /// Interface for MailService.
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Send feedback info
        /// </summary>
        /// <param name="feedback">Feedback info to send.</param>
        void Send(Feedback feedback);

        /// <summary>
        /// Send message info
        /// </summary>
        /// <param name="feedback">Message to send.</param>
        void Send(Mail feedback);
    }
}
