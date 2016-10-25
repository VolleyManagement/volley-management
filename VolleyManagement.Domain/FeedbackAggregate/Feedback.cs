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

        private FeedbackStatusEnum _status;

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
                        Resources.ValidationFeedbackUsersEmail,
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
                        Resources.ValidationFeedbackContent,
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
        public FeedbackStatusEnum Status
        {
            get
            {
                return _status;
            }

            set
            {
                if (FeedbackValidation.ValidateStatus(_status, value))
                {
                    throw new ArgumentException(
                        Resources.ValidationFeedbackStatus,
                        Resources.FeedbackStatusParam);
                }

                _status = value;
            }
        }

        /// <summary>
        /// Gets or sets update date of feedback.
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets admin name.
        /// </summary>
        public string AdminName { get; set; }
    }
}
