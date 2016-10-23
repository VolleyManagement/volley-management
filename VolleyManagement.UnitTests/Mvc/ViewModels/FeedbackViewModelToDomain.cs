namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Services.FeedbackService;

    /// <summary>
    /// View model feedback team class test.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class FeedbackViewModelToDomain
    {
        private const string TEST_MAIL = "test@gmail.com";
        private const string TEST_CONTENT = "Test content";

        /// <summary>
        /// ToDomain() method test.
        /// Does correct feedback view model mapped to domain model.
        /// </summary>
        [TestMethod]
        public void ToDomain_FeedbackViewModel_MappedToDomain()
        {
            // Arrange
            var testViewModel = new FeedbackMvcViewModelBuilder()
                .WithId(1)
                .WithEmail(TEST_MAIL)
                .WithContent(TEST_CONTENT)
                .Build();

            // Creates expected Feedback.
            // We have to assign fields Date and Status with default values
            // because FeedbackViewModel object doesn't has this field
            // but we want to compare it with Feedback object.
            var expected = new FeedbackBuilder()
                .WithId(1)
                .WithEmail(TEST_MAIL)
                .WithContent(TEST_CONTENT)
                .WithDate(DateTime.MinValue)
                .WithStatus(0)
                .Build();

            // Act
            var actual = testViewModel.ToDomain();

            // Assert
            TestHelper.AreEqual(expected, actual, new FeedbackComparer());
        }
    }
}