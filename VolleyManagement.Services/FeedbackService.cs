namespace VolleyManagement.Services
{
    using System;
    using Contracts;
    using Domain.FeedbackAggregate;

    /// <summary>
    /// Represents an implementation of IFeedbackService contract.
    /// </summary>
    public class FeedbackService : IFeedbackService
    {
        #region Fields

        private readonly IFeedbackRepository _feedbackService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackService"/> class.
        /// </summary>
        /// <param name="feedbackService">read the instance of feedbackService class</param>
        public FeedbackService(IFeedbackRepository feedbackService)
        {
            _feedbackService = feedbackService;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Creates feedback.
        /// </summary>
        /// <param name="feedbackToCreate">Feedback to create.</param>
        public void Create(Feedback feedbackToCreate)
        {
            if (feedbackToCreate == null)
            {
                throw new ArgumentNullException("feedback");
            }

            UpdateFeedbackTime(feedbackToCreate);
            _feedbackService.Add(feedbackToCreate);
            _feedbackService.UnitOfWork.Commit();
        }

        #endregion

        #region Privates

        private void UpdateFeedbackTime(Feedback feedbackToUpdate)
        {
            feedbackToUpdate.Date = DateTime.Now;
        }

        #endregion
    }
}