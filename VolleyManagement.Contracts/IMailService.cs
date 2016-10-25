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
        /// Send message info
        /// </summary>
        /// <param name="feedback">Message to send.</param>
        void Send(Mail feedback);
    }
}
