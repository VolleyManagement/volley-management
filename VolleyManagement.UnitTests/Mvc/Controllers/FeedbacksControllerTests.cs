namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Contracts;
    using Contracts.Authorization;
    using Domain.FeedbackAggregate;
    using Domain.UsersAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using UI.Areas.Mvc.Controllers;
    using UI.Areas.Mvc.ViewModels.FeedbackViewModel;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.FeedbackService;

    /// <summary>
    /// Feedbacks controller tests.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class FeedbacksControllerTests
    {
        #region Fields

        private const int TEST_ID = 1;
        private const int ANONYM_ID = -1;
        private const string CREATE_VIEW = "Create";
        private const string FEEDBACK_SENT_MESSAGE = "FeedbackSentMessage";
        private const string TEST_MAIL = "test@gmail.com";
        private const string TEST_CONTENT = "Test content";
        private const string TEST_ENVIRONMENT = "Test environment";
        private const string EXCEPTION_MESSAGE = "ValidationMessage";
        private const string CHECK_DATA_MESSAGE = "Data is not valid.";
        private const string CHECK_CAPTCHA_MESSAGE = "Please verify that you are not a robot.";
        private const string SUCCESS_SENT_MESSAGE = "Your Feedback has been sent successfully.";

        private Mock<IFeedbackService> _feedbackServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<ICurrentUserService> _currentUserServiceMock;
        private Mock<ICaptchaManager> _captchaManagerMock;

        #endregion

        #region Init

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _feedbackServiceMock = new Mock<IFeedbackService>();
            _userServiceMock = new Mock<IUserService>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _captchaManagerMock = new Mock<ICaptchaManager>();

            _captchaManagerMock.Setup(m => m.IsFormSubmit(It.IsAny<string>())).Returns(true);
        }

        #endregion

        #region CreateGetAction

        /// <summary>
        /// Test for create GET method.
        /// User email is empty if user is not authenticated.
        /// </summary>
        [TestMethod]
        public void
            CreateGetAction_UserIsNotAuthentificated_FeedbackHasEmptyEmailField()
        {
            // Arrange
            var feedback = CreateInvalidFeedback();
            SetupCurrentUserGetId(ANONYM_ID);

            var sut = BuildSUT();

            // Act
            sut.Create();

            // Assert
            Assert.AreEqual(feedback.UsersEmail, string.Empty);
        }

        /// <summary>
        /// Test for create GET method.
        /// User is authenticated. User email returned.
        /// </summary>
        [TestMethod]
        public void
            CreateGetAction_UserIsAuthentificated_UsersEmailPrepolulated()
        {
            // Arrange
            SetupCurrentUserGetId(TEST_ID);

            _userServiceMock.Setup(us => us.GetUser(TEST_ID))
                .Returns(new User { Email = TEST_MAIL });

            var sut = BuildSUT();

            // Act
            var feedback = TestExtensions
                .GetModel<FeedbackViewModel>(sut.Create());

            // Assert
            Assert.AreEqual(TEST_MAIL, feedback.UsersEmail);
        }

        #endregion

        #region CreatePostAction

        /// <summary>
        /// Test for create POST method.
        /// Feedback is incorrect, Create view is returned.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ModelIsInvalid_CheckDataMessageReturned()
        {
            // Arrange
            var feedback = CreateInvalidFeedback();

            var sut = BuildSUT();
            SetInvalidModelState(sut);

            // Act
            var result = sut.Create(feedback) as JsonResult;
            var returnedDataResult = result.Data as FeedbackMessageViewModel;

            // Assert
            Assert.AreEqual(CHECK_DATA_MESSAGE, returnedDataResult.ResultMessage);
        }

        /// <summary>
        /// Test for create POST method.
        /// Feedback is correct, feedback sent message returned.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ModelIsValid_SuccessSentMessageReturned()
        {
            // Arrange
            var sut = BuildSUT();
            var feedback = CreateValidFeedback();

            // Act
            var result = sut.Create(feedback) as JsonResult;
            var returnedDataResult = result.Data as FeedbackMessageViewModel;

            // Assert
            Assert.AreEqual(SUCCESS_SENT_MESSAGE, returnedDataResult.ResultMessage);
        }

        /// <summary>
        /// Test for Create POST method.
        /// Valid model passed. Feedback created.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ModelIsValid_FeedbackCreated()
        {
            // Arrange
            var sut = BuildSUT();
            var feedback = CreateValidFeedback();
            var expectedFeedback = CreateExpectedFeedback();

            Feedback actualFeedback = null;
            this._feedbackServiceMock.Setup(f => f.Create(It.IsAny<Feedback>()))
                .Callback<Feedback>(a => actualFeedback = a);

            // Act
            sut.Create(feedback);

            // Assert
            TestHelper.AreEqual(
                expectedFeedback,
                actualFeedback,
                new FeedbackComparer());
        }

        /// <summary>
        /// Test for Create POST method.
        /// While calling IFeedbackService method Create()
        /// argument exception is thrown, ModelState has changed.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ArgumentExceptionThrown_ModelChanged()
        {
            // Arrange
            var feedback = CreateValidFeedback();
            _feedbackServiceMock.Setup(f => f.Create(It.IsAny<Feedback>()))
                .Throws(new ArgumentException(EXCEPTION_MESSAGE));

            var sut = BuildSUT();

            // Act
            sut.Create(feedback);
            var res = sut.ModelState[EXCEPTION_MESSAGE].Errors;

            // Assert
            Assert.IsFalse(sut.ModelState.IsValid);
            Assert.AreEqual(EXCEPTION_MESSAGE, res[0].ErrorMessage);
        }

        [TestMethod]
        public void CreatePostAction_CaptchaIsNotApproved_CheckCaptchaMessageReturned()
        {
            // Arrange
            var feedback = CreateValidFeedback();
            var sut = BuildSUT();

            // Act
            _captchaManagerMock
                .Setup(cm => cm.IsFormSubmit(It.IsAny<string>()))
                .Returns(false);
            var res = sut.Create(feedback) as JsonResult;
            var returnedDataResult = res.Data as FeedbackMessageViewModel;

            // Assert
            Assert.AreEqual(CHECK_CAPTCHA_MESSAGE, returnedDataResult.ResultMessage);
        }
        #endregion

        #region Private

        private FeedbackViewModel CreateValidFeedback()
        {
            return
                new FeedbackMvcViewModelBuilder()
                    .WithId(TEST_ID)
                    .WithEmail(TEST_MAIL)
                    .WithContent(TEST_CONTENT)
                    .WithEnvironment(TEST_ENVIRONMENT)
                    .Build();
        }

        private Feedback CreateExpectedFeedback()
        {
            return
                new FeedbackBuilder()
                    .WithId(TEST_ID)
                    .WithEmail(TEST_MAIL)
                    .WithContent(TEST_CONTENT)
                    .WithEnvironment(TEST_ENVIRONMENT)
                    .WithDate(DateTime.MinValue)
                    .Build();
        }

        private FeedbackViewModel CreateInvalidFeedback()
        {
            return
                new FeedbackMvcViewModelBuilder()
                    .WithId(TEST_ID)
                    .WithEmail(string.Empty)
                    .WithContent(string.Empty)
                    .WithEnvironment(string.Empty)
                    .Build();
        }

        private FeedbacksController BuildSUT()
        {
            return new FeedbacksController(
                _feedbackServiceMock.Object,
                _userServiceMock.Object,
                _currentUserServiceMock.Object,
                _captchaManagerMock.Object);
        }

        private void SetInvalidModelState(FeedbacksController controller)
        {
            controller.ModelState.AddModelError("Content", "FieldRequired");
        }

        private void SetupCurrentUserGetId(int id)
        {
            this._currentUserServiceMock.Setup(cs => cs.GetCurrentUserId())
                .Returns(id);
        }

        #endregion
    }
}