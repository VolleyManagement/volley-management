namespace VolleyManagement.Services
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Web.Configuration;
    using VolleyManagement.Crosscutting.Contracts.MailService;

    /// <summary>
    /// Defines GmailAccountMailService.
    /// </summary>
    public class GmailAccountMailService : IMailService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GmailAccountMailService"/> class.
        /// </summary>
        public GmailAccountMailService()
        {
        }

        /// <summary>
        /// Send an email.
        /// </summary>
        /// <param name="message">Message to send.</param>
        public void Send(EmailMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(
                    Properties.Resources.ArgumentExceptionInvalidGmailMessage,
                    Properties.Resources.ArgumentExceptionInvalidGmailMessage);
            }

            SmtpClient smtp = GetSmtpClient(
                GetSenderEmailAddress(),
                GetSenderPassword());

            smtp.Send(GetSenderEmailAddress(), message.Recipient, message.Subject, message.Body);
        }

        private string GetSenderEmailAddress()
        {
            const string EMAIL_ADDRESS_KEY = "GoogleEmailAddress";

            var emailAddress = WebConfigurationManager.AppSettings[EMAIL_ADDRESS_KEY];

            if (emailAddress == null)
            {
                throw new ArgumentNullException(
                    Properties.Resources.ArgumentNullExceptionInvalidGmailAddress,
                    Properties.Resources.GmailAddress);
            }

            return emailAddress;
        }

        private string GetSenderPassword()
        {
            const string EMAIL_PASSWORD_KEY = "GoogleEmailPassword";

            var password = WebConfigurationManager.AppSettings[EMAIL_PASSWORD_KEY];

            if (password == null)
            {
                throw new ArgumentNullException(
                    Properties.Resources.ArgumentNullExceptionInvalidGmailPassword,
                    Properties.Resources.GmailPassword);
            }

            return password;
        }

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
