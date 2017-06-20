namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.FeedbackViewModel;

    /// <summary>
    /// Builder for test MVC feedback view model.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class FeedbackMvcViewModelBuilder
    {
        private const string TEST_MAIL = "test@gmail.com";
        private const string TEST_CONTENT = "Test content";
        private const string TEST_ENVIRONMENT = "Test environment";

        /// <summary>
        /// Holds test feedback team view model instance.
        /// </summary>
        private readonly FeedbackViewModel _feedbackViewModel;

       /// <summary>
       /// Initializes a new instance of the <see
       /// cref="FeedbackMvcViewModelBuilder"/> class.
       /// </summary>
        public FeedbackMvcViewModelBuilder()
        {
            _feedbackViewModel = new FeedbackViewModel
            {
                Id = 1,
                UsersEmail = TEST_MAIL,
                Content = TEST_CONTENT,
                UserEnvironment = TEST_ENVIRONMENT
            };
        }

        /// <summary>
        /// Sets the feedback view model Id.
        /// </summary>
        /// <param name="id">Feedback view model Id.</param>
        /// <returns>Feedback view model builder object.</returns>
        public FeedbackMvcViewModelBuilder WithId(int id)
        {
            _feedbackViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets the feedback view model email.
        /// </summary>
        /// <param name="email">Feedback view model email.</param>
        /// <returns>Feedback view model object.</returns>
        public FeedbackMvcViewModelBuilder WithEmail(string email)
        {
            _feedbackViewModel.UsersEmail = email;
            return this;
        }

        /// <summary>
        /// Sets the feedback view model content.
        /// </summary>
        /// <param name="content">Feedback view model content.</param>
        /// <returns>Feedback view model object.</returns>
        public FeedbackMvcViewModelBuilder WithContent(string content)
        {
            _feedbackViewModel.Content = content;
            return this;
        }

        /// <summary>
        /// Sets the feedback view model environment.
        /// </summary>
        /// <param name="environment">Feedback view model content.</param>
        /// <returns>Feedback view model object.</returns>
        public FeedbackMvcViewModelBuilder WithEnvironment(string environment)
        {
            _feedbackViewModel.UserEnvironment = environment;
            return this;
        }

        /// <summary>
        /// Builds test FeedbackViewModel.
        /// </summary>
        /// <returns>Test Feedback view model.</returns>
        public FeedbackViewModel Build()
        {
            return _feedbackViewModel;
        }
    }
}
