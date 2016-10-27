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

        private readonly Mock<IFeedbackService> _feedbackServiceMock =
            new Mock<IFeedbackService>();

        private readonly Mock<IUserService> _userServiceMock =
            new Mock<IUserService>();

        private readonly Mock<ICurrentUserService> _currentUserServiceMock =
            new Mock<ICurrentUserService>();

        private FeedbacksController _sut;

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
            this._sut = this._kernel.Get<FeedbacksController>();
            var feedback = CreateInvalidFeedback();
            SetupCurrentUserGetId(ANONYM_ID);

            // Act
            this._sut.Create();

            // Assert
            Assert.AreEqual(feedback.UsersEmail, string.Empty);
        }

        /// <summary>
        /// Test for create GET method.
        /// User is authenticated. User email returned.
        /// </summary>
        [TestMethod]
        public void
            CreateGetAction_UserIsAuthentificated_FeedbackEmailEqualsUsersEmail()
        {
            // Arrange
            this._sut = this._kernel.Get<FeedbacksController>();
            SetupCurrentUserGetId(TEST_ID);
            this._userServiceMock.Setup(us => us.GetUser(TEST_ID))
                .Returns(new User { Email = TEST_MAIL });

            // Act
            var feedback = TestExtensions
                .GetModel<FeedbackViewModel>(this._sut.Create());

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
            this._sut = this._kernel.Get<FeedbacksController>();
            var feedback = CreateInvalidFeedback();

            SetInvalidModelState();

            // Act
            var result = this._sut.Create(feedback) as ViewResult;

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
            this._sut = this._kernel.Get<FeedbacksController>();
            var feedback = CreateValidFeedback();

            // Act
            var result = this._sut.Create(feedback) as ViewResult;

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
            this._sut = this._kernel.Get<FeedbacksController>();
            var feedback = CreateValidFeedback();
            var expectedFeedback = CreateExpectedFeedback();

            Feedback actualFeedback = null;
            this._feedbackServiceMock.Setup(f => f.Create(It.IsAny<Feedback>()))
                .Callback<Feedback>(a => actualFeedback = a);

            // Act
            this._sut.Create(feedback);

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
            this._sut = this._kernel.Get<FeedbacksController>();
            var feedback = CreateInvalidFeedback();

            this._feedbackServiceMock.Setup(f => f.Create(It.IsAny<Feedback>()))
                .Throws(new ArgumentException());

            // Act
            this._sut.Create(feedback);

            // Assert
            Assert.IsFalse(this._sut.ModelState.IsValid);
        }

        [TestMethod]
        public void
            CreatePostAction_FeedbackViewModelMapsCorrectly_FeedbackCreated()
        {
            // Arrange
            var expected = CreateExpectedFeedback();
            var feedback = CreateValidFeedback();

            // Act
            var actual = feedback.ToDomain();

            // Assert
            TestHelper.AreEqual(expected, actual, new FeedbackComparer());
        }

        #endregion

        #region Private

        /// <summary>
        /// Creates valid Feedback object.
        /// </summary>
        /// <returns>FeedbackViewModel object.</returns>
        private FeedbackViewModel CreateValidFeedback()
        {
            return
                new FeedbackMvcViewModelBuilder()
                    .WithId(TEST_ID)
                    .WithEmail(TEST_MAIL)
                    .WithContent(TEST_CONTENT)
                    .Build();
        }

        /// <summary>
        /// Creates expected Feedback.
        /// We have to assign fields Date and Status with default values
        /// because FeedbackViewModel object doesn't has this field
        /// but we want to compare it with Feedback object.
        /// </summary>
        /// <returns>Feedback object.</returns>
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

        private void SetInvalidModelState()
        {
            this._sut.ModelState.AddModelError("Content", "FieldRequired");
        }

        private void SetupCurrentUserGetId(int id)
        {
            this._currentUserServiceMock.Setup(cs => cs.GetCurrentUserId())
                .Returns(id);
        }

        #endregion
    }
}