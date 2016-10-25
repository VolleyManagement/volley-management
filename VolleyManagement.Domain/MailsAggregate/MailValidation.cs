namespace VolleyManagement.Domain.MailsAggregate
{
    /// <summary>
    /// Class for mail validation.
    /// </summary>
    public class MailValidation
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
        /// Validates body of mail.
        /// </summary>
        /// <param name="content">Mail body for validation.</param>
        /// <returns>Validity of body.</returns>
        public static bool ValidateBody(string content)
        {
            return string.IsNullOrEmpty(content)
                || content.Length > Constants.Mail.MAX_CONTENT_LENGTH;
        }
    }
}
