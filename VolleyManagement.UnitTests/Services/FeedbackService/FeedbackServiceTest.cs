namespace VolleyManagement.UnitTests.Services.FeedbackService
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Contracts;
    using Crosscutting.Contracts.MailService;
    using Crosscutting.Contracts.Providers;
    using Data.Contracts;
    using Data.Queries.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Domain.FeedbackAggregate;
    using VolleyManagement.Services;
    

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class FeedbackServiceTest
    {
        public const int SPECIFIC_FEEDBACK_ID = 2;
        public const int CONTENT_NOT_ALLOWED_LENGTH
            = Domain.Constants.Feedback.MAX_CONTENT_LENGTH;

        public const int EMAIL_NOT_ALLOWED_LENGTH
            = Domain.Constants.Feedback.MAX_EMAIL_LENGTH;

        public const string CONTENT_EXCEPTION_MESSAGE
                = "Content can't be empty or contains more than {0} symbols";

        public const string EMAIL_EXCEPTION_MESSAGE
                = "Email can't be empty or contains more than {0} symbols";

        private readonly Mock<IUnitOfWork> _unitOfWorkMock
            = new Mock<IUnitOfWork>();

        private readonly Mock<IUserService> _userServiceMock
            = new Mock<IUserService>();

        private readonly Mock<IMailService> _mailServiceMock
            = new Mock<IMailService>();

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
            _kernel.Bind<IFeedbackRepository>()
                .ToConstant(_feedbackRepositoryMock.Object);
            _kernel.Bind<IUserService>()
                .ToConstant(_userServiceMock.Object);
            _kernel.Bind<IMailService>()
                .ToConstant(_mailServiceMock.Object);
            _feedbackRepositoryMock.Setup(fr => fr.UnitOfWork)
                .Returns(_unitOfWorkMock.Object);
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
            var actual = new FeedbackBuilder()
                .WithDate(_feedbackTestDate)
                .Build();
            var expected = new FeedbackBuilder()
                .WithDate(_feedbackTestDate)
                .Build();
            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Create(actual);

            // Assert
            AssertFeedbackAreEquals(expected, actual);
            VerifyCreateFeedback(
                actual,
                Times.Once(),
                "Parameter feedback is not equal to Instance of feedback");
            VerifySaveFeedback(
                actual,
                Times.Once(),
                "DB should not be updated");
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
        public void Create_ContentNotAllowedLength_ArgumentExceptionThrown()
        {
            // Arrange
            string invalidContent =
                GenerateTooLongText(Domain.Constants.Feedback.MAX_CONTENT_LENGTH + 1);
            string argExMessage
                = CreateExceptionMessage(
                    CONTENT_EXCEPTION_MESSAGE,
                    CONTENT_NOT_ALLOWED_LENGTH);
            var testFeedback = new FeedbackBuilder().Build();
            Exception exception = null;
            var sut = _kernel.Get<FeedbackService>();

            // Act
            try
            {
                testFeedback = new FeedbackBuilder()
                    .WithContent(invalidContent)
                    .Build();
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
        public void Create_UsersMailNotAllowedLength_ArgumentExceptionThrown()
        {
            // Arrange
            string invalidEmail =
                GenerateTooLongText(Domain.Constants.Feedback.MAX_EMAIL_LENGTH + 1);
            string argExMessage
                = CreateExceptionMessage(
                    EMAIL_EXCEPTION_MESSAGE,
                    EMAIL_NOT_ALLOWED_LENGTH);
            var testFeedback = new FeedbackBuilder().Build();
            Exception exception = null;
            var sut = _kernel.Get<FeedbackService>();

            // Act
            try
            {
                testFeedback = new FeedbackBuilder()
                .WithEmail(invalidEmail)
                .Build();
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
        public void Create_EmptyFeedbackContent_ArgumentExceptionThrown()
        {
            // Arrange
            string invalidFeedbackContent = string.Empty;
            string argExMessage
                = CreateExceptionMessage(
                    CONTENT_EXCEPTION_MESSAGE,
                    CONTENT_NOT_ALLOWED_LENGTH);
            var testFeedback = new FeedbackBuilder().Build();
            Exception exception = null;
            var sut = _kernel.Get<FeedbackService>();

            // Act
            try
            {
                testFeedback = new FeedbackBuilder()
                                        .WithContent(invalidFeedbackContent)
                                        .Build();
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
        public void Create_EmptyFeedbackUsersMail_ArgumentExceptionThrown()
        {
            string invalidFeedbackUserEmail = string.Empty;
            string argExMessage = CreateExceptionMessage(
                    EMAIL_EXCEPTION_MESSAGE,
                    EMAIL_NOT_ALLOWED_LENGTH);
            var testFeedback = new FeedbackBuilder().Build();
            Exception exception = null;
            var sut = _kernel.Get<FeedbackService>();

            // Act
            try
            {
                testFeedback = new FeedbackBuilder()
                    .WithEmail(invalidFeedbackUserEmail)
                    .Build();
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

        private bool FeedbacksAreEqual(Feedback x, Feedback y)
        {
            return new FeedbackComparer().Compare(x, y) == 0;
        }

        private void AssertFeedbackAreEquals(Feedback expected, Feedback actual)
        {
            TestHelper.AreEqual<Feedback>(expected, actual, new FeedbackComparer());
        }

        private void VerifyCreateFeedback(Feedback feedback, Times times, string message)
        {
            _feedbackRepositoryMock.Verify(
                pr => pr.Add(
                It.Is<Feedback>(f =>
                FeedbacksAreEqual(f, feedback))),
                times,
                message);
        }

        private void VerifySaveFeedback(Feedback feedback, Times times, string message)
        {
            _unitOfWorkMock.Verify(
                uow => uow.Commit(),
                times,
                message);
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

        private string GenerateTooLongText(int length)
        {
            return new string(
                  Enumerable.Repeat<char>(
                      'a', length)
                      .ToArray());
        }

        private string CreateExceptionMessage(string message, int length)
        {
            return string.Format(message, length);
        }
    }
}