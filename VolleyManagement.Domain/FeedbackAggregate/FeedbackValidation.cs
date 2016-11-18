namespace VolleyManagement.Domain.FeedbackAggregate
{
    /// <summary>
    /// Feedback validation class.
    /// </summary>
    public class FeedbackValidation
    {
        /// <summary>
        /// Validates user's email.
        /// </summary>
        /// <param name="usersEmail">User's email for validation.</param>
        /// <returns>Validity of user's email.</returns>
        public static bool ValidateUsersEmail(string usersEmail)
        {
            return string.IsNullOrEmpty(usersEmail)
                || usersEmail.Length > Constants.Feedback.MAX_EMAIL_LENGTH;
        }

        /// <summary>
        /// Validates content of feedback.
        /// </summary>
        /// <param name="content">Feedback content for validation.</param>
        /// <returns>Validity of content.</returns>
        public static bool ValidateContent(string content)
        {
            return string.IsNullOrEmpty(content)
                || content.Length > Constants.Feedback.MAX_CONTENT_LENGTH;
        }

        /// <summary>
        /// Validates user environment of feedback.
        /// </summary>
        /// <param name="environment">Feedback user environment for validation.</param>
        /// <returns>Validity of user environment.</returns>
        public static bool ValidateUserEnvironment(string environment)
        {
            bool result = false;
            if (environment != null)
            {
                result = environment.Length
                   > Constants.Feedback.MAX_USER_ENVIRONMENT_LENGTH;
            }

            return result;
        }
    }
}
