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
            this._sut = this._kernel.Get<FeedbacksController>();
        }

        /// <summary>
        /// Resets Mock setups after every test.
        /// </summary>
        [TestCleanup]
        public void ResetMocks()
        {
            this._currentUserServiceMock.Reset();
            this._feedbackServiceMock.Reset();
            this._userServiceMock.Reset();
        }

        /// <summary>
        /// Test for create GET method.
        /// User email is empty if user is not authenticated.
        /// </summary>
        [TestMethod]
        public void CreateGetAction_UserIsNotAuthentificated_FeedbackHasEmptyEmailField()
        {
            // Arrange
            var feedback = new FeedbackMvcViewModelBuilder().WithEmail(string.Empty).Build();
            this._currentUserServiceMock.Setup(cs => cs.GetCurrentUserId()).Returns(ANONYM_ID);

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
        public void CreateGetAction_UserIsAuthentificated_FeedbackEmailEqualsUsersEmail()
        {
            // Arrange
            var feedback = CreateValidFeedback();
            this._currentUserServiceMock.Setup(
                cs => cs.GetCurrentUserId()).Returns(TEST_ID);

            this._userServiceMock.Setup(
                us => us.GetCurrentUserInstance(TEST_ID)).Returns(
                new User { Email = TEST_MAIL });

            // Act
            this._sut.Create();

            // Assert
            Assert.AreEqual(feedback.UsersEmail, TEST_MAIL);
        }

        /// <summary>
        /// Test for create POST method.
        /// Feedback is incorrect, Create view is returned.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ModelIsInvalid_CreateViewReturned()
        {
            // Arrange
            var feedback = CreateValidFeedback();
            this._sut.ModelState.AddModelError("Content", "FieldRequired");

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
        /// argument exception is thrown.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_InvalidModel_ArgumentExceptionThrown()
        {
            // Arrange
            var expectedFeedback = new FeedbackMvcViewModelBuilder()
                .WithId(TEST_ID)
                .WithEmail(string.Empty)
                .WithContent(string.Empty)
                .Build();

            this._feedbackServiceMock.Setup(f => f.Create(It.IsAny<Feedback>()))
                .Throws(new ArgumentException());

            // Act
            var actualFeedback =
                TestExtensions.GetModel<FeedbackViewModel>(
                    this._sut.Create(expectedFeedback));

            // Assert
            TestHelper.AreEqual(
                actualFeedback,
                expectedFeedback,
                new FeedbackViewModelComparer());
        }

        /// <summary>
        /// Creates valid Feedback object.
        /// </summary>
        /// <returns>FeedbackViewModel object.</returns>
        private FeedbackViewModel CreateValidFeedback()
        {
            return new FeedbackMvcViewModelBuilder()
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
                new FeedbackBuilder().WithId(TEST_ID)
                    .WithEmail(TEST_MAIL)
                    .WithContent(TEST_CONTENT)
                    .WithDate(DateTime.MinValue)
                    .WithStatus(0)
                    .Build();
        }
    }
}