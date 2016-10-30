namespace VolleyManagement.Contracts
{
    using VolleyManagement.Crosscutting.MailService;

    /// <summary>
    /// Defines a contract for Mail service.
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Send an email.
        /// </summary>
        /// <param name="message">Email to send.</param>
        void Send(EmailMessage message);
    }
}
