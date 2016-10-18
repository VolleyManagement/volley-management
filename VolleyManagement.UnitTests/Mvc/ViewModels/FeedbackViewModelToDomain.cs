namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Services.FeedbackService;

    /// <summary>
    /// View model feedback team class test.
    /// </summary>
    [TestClass]
    public class FeedbackViewModelToDomain
    {
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
                .WithEmail("test@gmail.com")
                .WithContent("Test content")
                .Build();

            var testDomainModel = new FeedbackBuilder()
                .WithId(1)
                .WithEmail("test@gmail.com")
                .WithContent("Test content")
                .WithDate(DateTime.MinValue)
                .WithStatus(0)
                .Build();

            //// Act
            var actual = testViewModel.ToDomain();

            //// Assert
            TestHelper.AreEqual(testDomainModel, actual, new FeedbackComparer());
        }
    }
}