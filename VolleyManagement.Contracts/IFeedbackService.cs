namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;
    using Domain.FeedbackAggregate;
    using Domain.MailsAggregate;

    /// <summary>
    /// Defines a contract for Feedback service.
    /// </summary>
    public interface IFeedbackService
    {
        /// <summary>
        /// Gets list of all feedbacks.
        /// </summary>
        /// <returns>Return list of all teams.</returns>
        List<Feedback> Get();

        /// <summary>
        /// Find feedback by id.
        /// </summary>
        /// <param name="id">Team id.</param>
        /// <returns>Found team.</returns>
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
        /// <param name="answerToSend">Information about mail (body, receiver)</param>
         void Reply(int id, Mail answerToSend);
    }
}
