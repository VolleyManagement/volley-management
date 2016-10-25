namespace VolleyManagement.Domain.MailsAggregate
{
    using System;
    using VolleyManagement.Domain.Properties;

    /// <summary>
    /// Mail domain class.
    /// </summary>
    public class Mail
    {
        private string _to;
        private string _from;
        private string _body;

        /// <summary>
        /// Gets or sets the identifier of mail.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the receiver of the mail.
        /// </summary>
        public string To
        {
            get
            {
                return _to;
            }

            set
            {
                if (MailValidation.ValidateUsersEmail(value))
                {
                    throw new ArgumentException(
                        Resources.ValidationMailEmail,
                        Resources.MailToParam);
                }

                _to = value;
            }
        }

        /// <summary>
        /// Gets or sets the sender of the mail.
        /// </summary>
        public string From
        {
            get
            {
                return _from;
            }

            set
            {
                if (MailValidation.ValidateUsersEmail(value))
                {
                    throw new ArgumentException(
                        Resources.ValidationMailEmail,
                        Resources.MailFromParam);
                }

                _from = value;
            }
        }

        /// <summary>
        /// Gets or sets the body of the mail.
        /// </summary>
        public string Body
        {
            get
            {
                return _body;
            }

            set
            {
                if (MailValidation.ValidateBody(value))
                {
                    throw new ArgumentException(
                        Resources.ValidationMailBody,
                        Resources.MailBodyParam);
                }

                _body = value;
            }
        }
    }
}
