namespace VolleyManagement.Domain
{
    using System;

    /// <summary>
    /// Gmail message for sending emails.
    /// </summary>
    public class GmailMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GmailMessage"/> class.
        /// </summary>
        /// <param name="from">Sender of email.</param>
        /// <param name="recipient">Recipient of email.</param>
        /// <param name="subject">Subject of email.</param>
        /// <param name="body">Body of email.</param>
        public GmailMessage(string from, string recipient, string subject, string body)
        {
            if (string.IsNullOrEmpty(from)
                || string.IsNullOrEmpty(recipient)
                || string.IsNullOrEmpty(body)
                || string.IsNullOrEmpty(subject))
            {
                throw new ArgumentException(
                    Properties.Resources.ArgumentExceptionInvalidGmailMessageArguments,
                    Properties.Resources.ArgumentExceptionGmailMessage);
            }

            From = from;
            Body = body;
            Subject = subject;
            Recipient = recipient;
        }

        /// <summary>
        /// Sender of the email.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Body of the email.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Subject of the email.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Recipient of the email.
        /// </summary>
        public string Recipient { get; set; }
    }
}
