namespace VolleyManagement.Crosscutting.Contracts.MailService
{
    using System;

    /// <summary>
    /// Gmail message for sending emails.
    /// </summary>
    public class EmailMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailMessage"/> class.
        /// </summary>
        /// <param name="recipient">Recipient of email.</param>
        /// <param name="subject">Subject of email.</param>
        /// <param name="body">Body of email.</param>
        public EmailMessage(string recipient, string subject, string body)
        {
            if (string.IsNullOrEmpty(recipient)
                || string.IsNullOrEmpty(body)
                || string.IsNullOrEmpty(subject))
            {
                throw new ArgumentException(
                    Properties.Resources.ArgumentExceptionInvalidEmailMessageArguments,
                    Properties.Resources.ArgumentExceptionEmailMessage);
            }

            Body = body;
            Subject = subject;
            Recipient = recipient;
        }

        /// <summary>
        /// Gets or sets body of the email.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets subject of the email.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets recipient of the email.
        /// </summary>
        public string Recipient { get; set; }
    }
}
