namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using Contracts;
    using Contracts.Authorization;
    using Domain.FeedbackAggregate;
    using Domain.UsersAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using Services.FeedbackService;
    using UI.Areas.Mvc.Controllers;
    using UI.Areas.Mvc.ViewModels.FeedbackViewModel;
    using ViewModels;

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
        private const string EXCEPTION_KEY = "ValidationMessage";
        private const string EXCEPTION_MESSAGE = "Please, enter the valid email\r\nParameter name: UsersEmail";

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
            this._kernel = new StandardKernel();
            this._kernel.Bind<IFeedbackService>()
                .ToConstant(this._feedbackServiceMock.Object);
            this._kernel.Bind<IUserService>()
                .ToConstant(this._userServiceMock.Object);
            this._kernel.Bind<ICurrentUserService>()
                .ToConstant(this._currentUserServiceMock.Object);
            this._kernel.Bind<ICaptchaManager>()
                .ToConstant(this._captchaManagerMock.Object);

            _captchaManagerMock.Setup(m => m.IsFormSubmit(It.IsAny<HttpRequestBase>())).Returns(true);
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
            FeedbacksController sut = this._kernel.Get<FeedbacksController>();
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
            FeedbacksController sut = this._kernel.Get<FeedbacksController>();
            SetupCurrentUserGetId(TEST_ID);
            this._userServiceMock.Setup(us => us.GetUser(TEST_ID))
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
        public void CreatePostAction_ModelIsInvalid_CreateViewReturned()
        {
            // Arrange
            FeedbacksController sut = this._kernel.Get<FeedbacksController>();
            var feedback = CreateInvalidFeedback();

            SetInvalidModelState(sut);

            // Act
            var result = sut.Create(feedback) as ViewResult;

            // Assert
            Assert.AreEqual(CREATE_VIEW, result.ViewName);
        }

        /// <summary>
        /// Test for create POST method.
        /// Feedback is correct, feedback sent message returned.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ModelIsValid_FeedbackThankReturned()
        {
            // Arrange
            FeedbacksController sut = this._kernel.Get<FeedbacksController>();
            var feedback = CreateValidFeedback();

            // Act
            var result = sut.Create(feedback) as ViewResult;

            // Assert
            Assert.AreEqual(FEEDBACK_SENT_MESSAGE, result.ViewName);
        }

        /// <summary>
        /// Test for Create POST method.
        /// Valid model passed. Feedback created.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ModelIsValid_FeedbackCreated()
        {
            // Arrange
            FeedbacksController sut = this._kernel.Get<FeedbacksController>();
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
            FeedbacksController sut = this._kernel.Get<FeedbacksController>();
            var feedback = CreateInvalidFeedback();

            this._feedbackServiceMock.Setup(f => f.Create(It.IsAny<Feedback>()))
                .Throws(new ArgumentException(EXCEPTION_KEY));

            // Act
            sut.Create(feedback);
            var res = sut.ModelState[EXCEPTION_KEY].Errors;

            // Assert
            Assert.IsFalse(sut.ModelState.IsValid);
            Assert.AreEqual(EXCEPTION_MESSAGE, res[0].ErrorMessage);
        }

        [TestMethod]
        public void CreatePostAction_CaptchaIsNotApproved_CreateViewReturned()
        {
            // Arrange
            FeedbacksController sut = this._kernel.Get<FeedbacksController>();
            var feedback = CreateValidFeedback();

            // Act
            this._captchaManagerMock
                .Setup(cm => cm.IsFormSubmit(It.IsAny<HttpRequestBase>()))
                .Returns(false);
            var res = sut.Create(feedback) as ViewResult;

            // Assert
            Assert.AreNotEqual(FEEDBACK_SENT_MESSAGE, res.ViewName);
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
                    .Build();
        }

        private Feedback CreateExpectedFeedback()
        {
            return
                new FeedbackBuilder()
                    .WithId(TEST_ID)
                    .WithEmail(TEST_MAIL)
                    .WithContent(TEST_CONTENT)
                    .WithDate(DateTime.MinValue)
                    .WithStatus(0)
                    .Build();
        }

        private FeedbackViewModel CreateInvalidFeedback()
        {
            return
                new FeedbackMvcViewModelBuilder()
                    .WithId(TEST_ID)
                    .WithEmail(string.Empty)
                    .WithContent(string.Empty)
                    .Build();
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