namespace VolleyManagement.Contracts
{
    using Domain;

    /// <summary>
    /// Defines a contract for Mail service.
    /// </summary>
    public interface IGmailAccountMailService
    {
        /// <summary>
        /// Send an email.
        /// </summary>
        /// <param name="message">Email to send.</param>
        void Send(GmailMessage message);
    }
}
