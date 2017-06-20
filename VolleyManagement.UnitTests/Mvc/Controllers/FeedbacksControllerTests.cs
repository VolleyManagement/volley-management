namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Domain.FeedbackAggregate;
    using VolleyManagement.Domain.UsersAggregate;
    using VolleyManagement.UI.Areas.Mvc.Controllers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.FeedbackViewModel;
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

        private readonly Mock<IFeedbackService> _feedbackServiceMock =
            new Mock<IFeedbackService>();

        private readonly Mock<IUserService> _userServiceMock =
            new Mock<IUserService>();

        private readonly Mock<ICurrentUserService> _currentUserServiceMock =
            new Mock<ICurrentUserService>();

        private readonly Mock<ICaptchaManager> _captchaManagerMock =
            new Mock<ICaptchaManager>();

        private IKernel _kernel;

        #endregion

        #region Init

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IFeedbackService>()
                .ToConstant(_feedbackServiceMock.Object);
            _kernel.Bind<IUserService>()
                .ToConstant(_userServiceMock.Object);
            _kernel.Bind<ICurrentUserService>()
                .ToConstant(_currentUserServiceMock.Object);
            _kernel.Bind<ICaptchaManager>()
                .ToConstant(_captchaManagerMock.Object);

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
            FeedbacksController sut = _kernel.Get<FeedbacksController>();
            var feedback = CreateInvalidFeedback();
            SetupCurrentUserGetId(ANONYM_ID);

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
            FeedbacksController sut = _kernel.Get<FeedbacksController>();
            SetupCurrentUserGetId(TEST_ID);
            _userServiceMock.Setup(us => us.GetUser(TEST_ID))
                .Returns(new User { Email = TEST_MAIL });

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
            FeedbacksController sut = _kernel.Get<FeedbacksController>();
            var feedback = CreateInvalidFeedback();

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
            FeedbacksController sut = _kernel.Get<FeedbacksController>();
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
            FeedbacksController sut = _kernel.Get<FeedbacksController>();
            var feedback = CreateValidFeedback();
            var expectedFeedback = CreateExpectedFeedback();

            Feedback actualFeedback = null;
            _feedbackServiceMock.Setup(f => f.Create(It.IsAny<Feedback>()))
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
            FeedbacksController sut = _kernel.Get<FeedbacksController>();
            var feedback = CreateValidFeedback();
            _feedbackServiceMock.Setup(f => f.Create(It.IsAny<Feedback>()))
                .Throws(new ArgumentException(EXCEPTION_MESSAGE));

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
            FeedbacksController sut = _kernel.Get<FeedbacksController>();
            var feedback = CreateValidFeedback();

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

        private void SetInvalidModelState(FeedbacksController controller)
        {
            controller.ModelState.AddModelError("Content", "FieldRequired");
        }

        private void SetupCurrentUserGetId(int id)
        {
            _currentUserServiceMock.Setup(cs => cs.GetCurrentUserId())
                .Returns(id);
        }

        #endregion
    }
}