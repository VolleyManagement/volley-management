namespace VolleyManagement.Domain.MailsAggregate
{
    /// <summary>
    /// Represents a domain model of mail.
    /// </summary>
    public class Mail
    {
        /// <summary>
        /// Gets or sets the identifier of mail.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the receiver of the mail.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Gets or sets the sender of the mail.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the body of the mail.
        /// </summary>
        public string Body { get; set; }
    }
}
