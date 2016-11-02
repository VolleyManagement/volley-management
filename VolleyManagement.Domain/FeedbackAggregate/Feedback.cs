namespace VolleyManagement.Domain.FeedbackAggregate
{
    using System;
    using Properties;

    /// <summary>
    /// Represents domain model of feedback.
    /// </summary>
    public class Feedback
    {
        private string _usersEmail;
        private string _content;

        /// <summary>
        /// Gets or sets feedback Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets users email.
        /// </summary>
        public string UsersEmail
        {
            get
            {
                return _usersEmail;
            }

            set
            {
                if (FeedbackValidation.ValidateUsersEmail(value))
                {
                    throw new ArgumentException(
                        string.Format(
                            Resources.ValidationFeedbackUsersEmail,
                            VolleyManagement.Domain.Constants.Feedback.MAX_EMAIL_LENGTH),
                            Resources.FeedbackUsersEmailParam);
                }

                _usersEmail = value;
            }
        }

        /// <summary>
        /// Gets or sets feedback content.
        /// </summary>
        public string Content
        {
            get
            {
                return _content;
            }

            set
            {
                if (FeedbackValidation.ValidateContent(value))
                {
                    throw new ArgumentException(
                         string.Format(
                            Resources.ValidationFeedbackContent,
                            VolleyManagement.Domain.Constants.Feedback.MAX_CONTENT_LENGTH),
                            Resources.FeedbackContentParam);
                }

                _content = value;
            }
        }

        /// <summary>
        /// Gets or sets date of feedback.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets feedback status.
        /// </summary>
        public FeedbackStatusEnum Status { get; set; }

        /// <summary>
        /// Gets or sets environment (browser and operating system)
        /// of user's computer.
        /// </summary>
        public string UserEnvironment { get; set; }
    }
}
