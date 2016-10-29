﻿namespace VolleyManagement.Services
{
    using System;
    using Contracts;
    using Crosscutting.Contracts.Providers;
    using Domain.FeedbackAggregate;
    using Domain.Properties;

    /// <summary>
    /// Represents an implementation of IFeedbackService contract.
    /// </summary>
    public class FeedbackService : IFeedbackService
    {
        #region Fields

        private readonly IFeedbackRepository _feedbackRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackService"/> class.
        /// </summary>
        /// <param name="feedbackRepository"> The feedback repository</param>
        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
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

            UpdateFeedbackDate(feedbackToCreate);

            ValidateFeedback(feedbackToCreate);
            _feedbackRepository.Add(feedbackToCreate);
            _feedbackRepository.UnitOfWork.Commit();
        }

        #endregion

        #region Privates

        private void UpdateFeedbackDate(Feedback feedbackToUpdate)
        {
            feedbackToUpdate.Date = TimeProvider.Current.UtcNow;
        }

        private void ValidateContent(string feedbackContent)
        {
            if (FeedbackValidation.ValidateContent(feedbackContent))
            {
                throw new ArgumentException(
                    string.Format(
                    Resources.ValidationFeedbackContent,
                    VolleyManagement.Domain.Constants.Feedback.MAX_CONTENT_LENGTH),
                    "Content");
            }
        }
        

        private void ValidateEMail(string feedbackUserEMail)
        {
            if (FeedbackValidation.ValidateUsersEmail(feedbackUserEMail))
            {
                throw new ArgumentException(
                    string.Format(
                        Resources.ValidationFeedbackUsersEmail,
                        VolleyManagement.Domain.Constants.Feedback.MAX_EMAIL_LENGTH),
                    "UsersEmail");
            }
        }
             
        private void ValidateFeedback(Feedback feedbackToValidate)
        {
            ValidateContent(feedbackToValidate.Content);
            ValidateEMail(feedbackToValidate.UsersEmail);
        }
        #endregion
    }
}