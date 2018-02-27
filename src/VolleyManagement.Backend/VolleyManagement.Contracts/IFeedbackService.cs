namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;
    using Domain.FeedbackAggregate;

    /// <summary>
    /// Defines a contract for Feedback service.
    /// </summary>
    public interface IFeedbackService
    {
        /// <summary>
        /// Gets list of all feedbacks.
        /// </summary>
        /// <returns>Return list of all feedbacks.</returns>
        IEnumerable<Feedback> Get();

        /// <summary>
        /// Find feedback by id.
        /// </summary>
        /// <param name="id">Feedback id.</param>
        /// <returns>Found feedback.</returns>
        Feedback Get(int id);

        /// <summary>
        /// Create a new feedback
        /// </summary>
        /// <param name="feedbackToCreate">Feedback to create</param>
        void Create(Feedback feedbackToCreate);

        /// <summary>
        /// Get a Feedback's details by id.
        /// </summary>
        /// <param name="id">id for details.</param>
        /// <returns>Found feedback.</returns>
        Feedback GetDetails(int id);

        /// <summary>
        /// Close a Feedback.
        /// </summary>
        /// <param name="id">id for close.</param>
        void Close(int id);

        /// <summary>
        /// Reply the answer to user.
        /// </summary>
        /// <param name="id">id for reply.</param>
        /// <param name="message">message for reply</param>
        void Reply(int id, string message);
    }
}
