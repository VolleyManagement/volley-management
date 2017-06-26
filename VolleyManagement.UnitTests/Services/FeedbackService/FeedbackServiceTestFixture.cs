namespace VolleyManagement.UnitTests.Services.FeedbackService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.FeedbackAggregate;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FeedbackServiceTestFixture
    {
        /// <summary>
        /// Holds collection of Feedbacks
        /// </summary>
        private List<Feedback> _feedbacks = new List<Feedback>();

        /// <summary>
        /// Return test collection of Feedbacks
        /// </summary>
        /// <returns>Builder object with collection of Feedbacks</returns>
        public FeedbackServiceTestFixture TestFeedbacks()
        {
            _feedbacks.Add(new Feedback()
            {
                Id = 1,
                UsersEmail = "hello@gmail.com",
                Content = "Have some problem",
                Date = new DateTime(2016, 10, 10)
            });
            _feedbacks.Add(new Feedback()
            {
                Id = 2,
                UsersEmail = "helloworld@gmail.com",
                Content = "Have some problem with feedback",
                Date = new DateTime(2016, 10, 11)
            });

            return this;
        }

        /// <summary>
        /// Add feedback to collection.
        /// </summary>
        /// <param name="newFeedback">Feedback to add.</param>
        /// <returns>Builder object with collection of Feedbacks.</returns>
        public FeedbackServiceTestFixture AddFeedback(Feedback newFeedback)
        {
            _feedbacks.Add(newFeedback);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Feedback collection</returns>
        public List<Feedback> Build()
        {
            return _feedbacks;
        }
    }
}