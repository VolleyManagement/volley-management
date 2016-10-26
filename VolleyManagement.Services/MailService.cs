namespace VolleyManagement.Services
{
    using System;
    using VolleyManagement.Contracts;
    using VolleyManagement.Domain.MailsAggregate;

    /// <summary>
    /// Defines an implementation of <see cref="IMailService"/> contract.
    /// </summary>
    public class MailService : IMailService
    {
        /// <summary>
        /// Send email.
        /// </summary>
        /// <param name="mail">Mail to send.</param>
        public void Send(Mail mail)
        {
        }
    }
}
