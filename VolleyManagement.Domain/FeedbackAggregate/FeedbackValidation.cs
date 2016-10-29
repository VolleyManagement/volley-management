namespace VolleyManagement.Domain.FeedbackAggregate
{
    /// <summary>
    /// Feedback validation class
    /// </summary>
    public class FeedbackValidation
    {
        /// <summary>
        /// Validate feedback content
        /// </summary>
        /// <param name="feedbackContent"> Feedback content for validation</param>
        /// <returns> Validity of feedback content</returns>
        public static bool ValidateFeedbackContent(string feedbackContent)
        {
            return string.IsNullOrEmpty(feedbackContent) || feedbackContent.Length > Constants.Feedback.MAX_CONTENT_LENGTH;
        }
    }
}
