﻿namespace VolleyManagement.UnitTests.Services.FeedbackService
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Domain.FeedbackAggregate;

    /// <summary>
    /// Represents a builder of <see cref="Feedback"/> objects for unit
    /// tests for <see cref="FeedbackService"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class FeedbackBuilder
    {
        private readonly Feedback _feedback;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackBuilder"/> class.
        /// </summary>
        public FeedbackBuilder()
        {
            _feedback = new Feedback
            {
                Id = 1,
                UsersEmail = "qwerty@gmail.com",
                Content = "A perfect application",
                Date = new DateTime(2007, 05, 03),
                Status = FeedbackStatusEnum.New,
                UserEnvironment = "Windows10"
            };
        }

        /// <summary>
        /// Sets the identifier of the feedback.
        /// </summary>
        /// <param name="id">Identifier of the feedback.</param>
        /// <returns>Instance of <see cref="FeedbackBuilder"/>.</returns>
        public FeedbackBuilder WithId(int id)
        {
            _feedback.Id = id;
            return this;
        }

        /// <summary>
        /// Sets the user email.
        /// </summary>
        /// <param name="email">User email</param>
        /// <returns>Feedback object</returns>
        public FeedbackBuilder WithEmail(string email)
        {
            _feedback.UsersEmail = email;
            return this;
        }

        /// <summary>
        /// Sets the feedback content.
        /// </summary>
        /// <param name="content">Feedback content</param>
        /// <returns>Feedback object</returns>
        public FeedbackBuilder WithContent(string content)
        {
            _feedback.Content = content;
            return this;
        }

        /// <summary>
        /// Sets the feedback date
        /// </summary>
        /// <param name="date">Feedback date</param>
        /// <returns>Feedback object</returns>
        public FeedbackBuilder WithDate(DateTime date)
        {
            _feedback.Date = date;
            return this;
        }

        /// <summary>
        /// Sets the feedback status
        /// </summary>
        /// <param name="status">Feedback status</param>
        /// <returns>Feedback object</returns>
        public FeedbackBuilder WithStatus(FeedbackStatusEnum status)
        {
            _feedback.Status = status;
            return this;
        }

        /// <summary>
        /// Sets the feedback environment.
        /// </summary>
        /// <param name="environment">Feedback environment</param>
        /// <returns>Feedback object</returns>
        public FeedbackBuilder WithEnvironment(string environment)
        {
            _feedback.UserEnvironment = environment;
            return this;
        }

        /// <summary>
        /// Builds test feedback.
        /// </summary>
        /// <returns>Test feedback</returns>
        public Feedback Build()
        {
            return _feedback;
        }
    }
}
