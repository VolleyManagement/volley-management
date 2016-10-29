namespace VolleyManagement.UnitTests.Services.FeedbackService
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Crosscutting.Contracts.Providers;
    using Data.Contracts;
    using Data.Queries.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Domain.FeedbackAggregate;
    using VolleyManagement.Domain.Properties;
    using VolleyManagement.Services;
    using System.Text;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class FeedbackServiceTest
    {
        public const int SPECIFIC_FEEDBACK_ID = 2;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock
            = new Mock<IUnitOfWork>();

        private readonly Mock<TimeProvider> _timeMock = new Mock<TimeProvider>();

        private readonly Mock<IFeedbackRepository> _feedbackRepositoryMock
            = new Mock<IFeedbackRepository>();

        private readonly Mock<IQuery<Feedback, FindByIdCriteria>> _getFeedbackByIdQueryMock =
            new Mock<IQuery<Feedback, FindByIdCriteria>>();

        private IKernel _kernel;
        private DateTime _feedbackTestDate = new DateTime(2007, 05, 03);

        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IFeedbackRepository>().ToConstant(_feedbackRepositoryMock.Object);
            _feedbackRepositoryMock.Setup(fr => fr.UnitOfWork).Returns(_unitOfWorkMock.Object);
            TimeProvider.Current = _timeMock.Object;
            _timeMock.Setup(tp => tp.UtcNow).Returns(_feedbackTestDate);
        }

        /// <summary>
        /// Cleanup test data
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            TimeProvider.ResetToDefault();
        }

        /// <summary>
        /// Test for Create() method. The method should create a new feedback.
        /// </summary>
        [TestMethod]
        public void Create_FeedbackPassed_FeedbackCreated()
        {
            // Arrange
            var newFeedback = new FeedbackBuilder().Build();
            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Create(newFeedback);

            // Assert
            VerifyCreateFeedback(
                newFeedback,
                Times.Once(),
                "Parameter feedback is not equal to Instance of feedback");
        }

        [TestMethod]
        public void Create_InvalidNullFeedback_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            Exception exception = null;
            Feedback newFeedback = null;
            var sut = _kernel.Get<FeedbackService>();

            // Act
            try
            {
                sut.Create(newFeedback);
            }
            catch (ArgumentNullException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                new ArgumentNullException("feedback"));
        }

        [TestMethod]
        public void Create_InvalidFeedbackContentNotAllowedLength_ArgumentExceptionThrown()
        {
            // Arrange
            string invalidContent = CreateInvalidFeedbackContent();
            string argExMessage = string.Format(
                Resources.ValidationFeedbackContent,
                VolleyManagement.Domain.Constants.Feedback.MAX_CONTENT_LENGTH);
            var testFeedback = new FeedbackBuilder()
                .WithContent(invalidContent)
                .Build();
            Exception exception = null;
            var sut = _kernel.Get<FeedbackService>();

            // Act
            try
            {
                sut.Create(testFeedback);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                new ArgumentException(argExMessage, "Content"));
        }

        [TestMethod]
        public void Create_InvalidFeedbackUsersMailNotAllowedLength_ArgumentExceptionThrown()
        {
            // Arrange
            string invalidEmail = CreateInvalidUsersEmail();
            string argExMessage = string.Format(
                Resources.ValidationFeedbackUsersEmail,
                VolleyManagement.Domain.Constants.Feedback.MAX_EMAIL_LENGTH);
            var testFeedback = new FeedbackBuilder()
                .WithEmail(invalidEmail)
                .Build();
            Exception exception = null;
            var sut = _kernel.Get<FeedbackService>();

            // Act
            try
            {
                sut.Create(testFeedback);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                new ArgumentException(argExMessage, "UsersEmail"));
        }

        [TestMethod]
        public void Create_EmptyFeedbackContent_FeedbackCreated()
        {
            // Arrange
            string invalidFeedbackContent = string.Empty;
            string argExMessage = string.Format(
                    Resources.ValidationFeedbackContent,
                        VolleyManagement.Domain.Constants.Feedback.MAX_CONTENT_LENGTH);
            var testFeedback = new FeedbackBuilder()
                                        .WithContent(invalidFeedbackContent)
                                        .Build();
            Exception exception = null;
            var sut = _kernel.Get<FeedbackService>();

            // Act
            try
            {
                sut.Create(testFeedback);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                new ArgumentException(argExMessage, "Content"));
        }

        [TestMethod]
        public void Create_EmptyFeedbackUsersEMail_FeedbackCreated()
        {
            string invalidFeedbackUserEmail = string.Empty;
            string argExMessage = string.Format(
                    Resources.ValidationFeedbackUsersEmail,
                        VolleyManagement.Domain.Constants.Feedback.MAX_EMAIL_LENGTH);
            var testFeedback = new FeedbackBuilder()
                                        .WithEmail(invalidFeedbackUserEmail)
                                        .Build();
            Exception exception = null;
            var sut = _kernel.Get<FeedbackService>();

            // Act
            try
            {
                sut.Create(testFeedback);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                new ArgumentException(argExMessage, "UsersEmail"));
        }

        [TestMethod]
        public void Get_DefaultFeedback_FeedbackDateReceived()
        {
            // Arrange
            var feed = new FeedbackBuilder().WithDate(_feedbackTestDate).Build();
            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Create(feed);

            // Assert
            Assert.AreEqual(TimeProvider.Current.UtcNow, feed.Date);
        }

        private bool FeedbacksAreEqual(Feedback x, Feedback y)
        {
            return new FeedbackComparer().Compare(x, y) == 0;
        }

        private void VerifyCreateFeedback(Feedback feedback, Times times, string message)
        {
            _feedbackRepositoryMock.Verify(
                pr => pr.Add(
                It.Is<Feedback>(f =>
                FeedbacksAreEqual(f, feedback))),
                times,
                message);
            _unitOfWorkMock.Verify(
                uow => uow.Commit(),
                times,
                "Can't save feedback to database");
        }

        private void VerifyExceptionThrown(Exception exception, Exception expected)
        {
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception.Message.Equals(expected.Message));
        }

        private void MockGetFeedbackByIdQuery(Feedback feedback)
        {
            _getFeedbackByIdQueryMock.Setup(fb => fb.Execute(It.IsAny<FindByIdCriteria>())).Returns(feedback);
        }

        private string CreateInvalidFeedbackContent()
        {
            StringBuilder invalidFeedbackContent = new StringBuilder();
            for (int i = 0; i < VolleyManagement.Domain.Constants.Feedback.MAX_CONTENT_LENGTH + 1; i++)
            {
                invalidFeedbackContent.Append("a");
            }

            return invalidFeedbackContent.ToString();
        }
        private string CreateInvalidUsersEmail()
        {
            StringBuilder invalidFeedbackUsersMail = new StringBuilder();
            for (int i = 0; i < VolleyManagement.Domain.Constants.Feedback.MAX_EMAIL_LENGTH + 1; i++)
            {
                invalidFeedbackUsersMail.Append("a");
            }

            return invalidFeedbackUsersMail.ToString();
        }
    }
}