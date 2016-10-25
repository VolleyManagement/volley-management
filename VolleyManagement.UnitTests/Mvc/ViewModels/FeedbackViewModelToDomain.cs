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
        /// Does correct feedback team view model mapped to domain model.
        /// </summary>
        [TestMethod]
        public void ToDomain_FeedbackViewModelMapped_ToDomain()
        {
            //// Arrange
            var testViewModel = new FeedbackMvcViewModelBuilder()
                .WithId(1)
                .WithEmail(TEST_MAIL)
                .WithContent(TEST_CONTENT)
                .Build();

            var testDomainModel = new FeedbackBuilder()
                .WithId(1)
                .WithEmail(TEST_MAIL)
                .WithContent(TEST_CONTENT)
                .WithDate(DateTime.MinValue)
                .Build();

            //// Act
            var actual = testViewModel.ToDomain();

            //// Assert
            TestHelper.AreEqual(testDomainModel, actual, new FeedbackComparer());
        }
    }
}