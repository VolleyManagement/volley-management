namespace VolleyManagement.Services
{
    using Contracts;
    using Domain.FeedbackAggregate;
    using System;

    /// <summary>
    /// Represents an implementation of IFeedbackService contract.
    /// </summary>
    public class FeedbackService : IFeedbackService
    {
        #region Fields

        private readonly IFeedbackRepository _feedbackService;

        #endregion

        #region Constructor

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