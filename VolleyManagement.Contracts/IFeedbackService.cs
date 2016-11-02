namespace VolleyManagement.Contracts
{
    using Domain.FeedbackAggregate;

    /// <summary>
    /// Defines a contract for Feedback service.
    /// </summary>
    public interface IFeedbackService
    {
        /// <summary>
        /// Create a new feedback
        /// </summary>
        /// <param name="feedbackToCreate">Feedback to create</param>
        void Create(Feedback feedbackToCreate);
    }
}
