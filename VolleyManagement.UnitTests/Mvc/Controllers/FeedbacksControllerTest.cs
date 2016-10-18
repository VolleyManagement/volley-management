namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    using Contracts;
    using Contracts.Authentication;
    using Contracts.Authorization;
    using Domain.FeedbackAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using Services.FeedbackService;
    using Services.UserManager;
    using UI.Areas.Mvc.Controllers;
    using ViewModels;

    /// <summary>
    /// Feedbacks controller tests.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class FeedbacksControllerTests
    {
        private const int ANONYM = -1;
        private const string CREATE_VIEW = "Create";
        private const string FEEDBACK_SENT_MESSAGE = "FeedbackSentMessage";
        private const string TEST_MAIL = "test@gmail.com";

        private readonly Mock<IFeedbackService> _feedbackServiceMock =
            new Mock<IFeedbackService>();

        private readonly Mock<IVolleyUserStore> _userStoreMock =
            new Mock<IVolleyUserStore>();

        private readonly Mock<ICurrentUserService> _userServiceMock =
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
            this._kernel.Bind<IVolleyUserStore>()
                .ToConstant(this._userStoreMock.Object);
            this._kernel.Bind<ICurrentUserService>()
                .ToConstant(this._userServiceMock.Object);
            this._sut = this._kernel.Get<FeedbacksController>();
        }

        /// <summary>
        /// Test for create GET method.
        /// Return Create view if user is not authenticated.
        /// </summary>
        [TestMethod]
        public void CreateGetAction_UserIsNotAuthentificated_CreateViewReturned()
        {
            ////Arrange
            var user = new UserMvcViewModelBuilder()
                .WithId(ANONYM)
                .Build();

            this._userServiceMock.Setup(us => us.GetCurrentUserId())
                .Returns(user.Id);

            ////Act
            var result = this._sut.Create() as ViewResult;

            ////Assert
            Assert.AreEqual(CREATE_VIEW, result.ViewName);
        }

        /// <summary>
        /// Test for create POST method.
        /// Feedback is incorrect, Create view is returned.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ModelIsInvalid_CreateViewReturned()
        {
            ////Arrange
            var feedback = new FeedbackMvcViewModelBuilder().Build();

            ////Act
            this._sut.ModelState.AddModelError("Content", "FieldRequired");
            var result = this._sut.Create(feedback) as ViewResult;

            ////Assert
            Assert.AreEqual(CREATE_VIEW, result.ViewName);
        }

        /// <summary>
        /// Test for create POST method.
        /// Feedback is correct, feedback sent message returned.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ModelIsValid_FeedbackThankReturned()
        {
            ////Arrange
            var feedback = new FeedbackMvcViewModelBuilder().Build();

            ////Act
            var result = this._sut.Create(feedback) as ViewResult;

            ////Assert
            Assert.AreEqual(FEEDBACK_SENT_MESSAGE, result.ViewName);
        }

        /// <summary>
        /// Test for Create POST method.
        /// Valid model passed. Feedback created.
        /// </summary>
        [TestMethod]
        public void CreatePostAction_ModelIsValid_FeedbackCreated()
        {
            ////Arrange
            var feedback = new FeedbackMvcViewModelBuilder().Build();
            Feedback expectedFeedback = feedback.ToDomain();

            Feedback actualFeedback = null;
            this._feedbackServiceMock.Setup(f => f.Create(It.IsAny<Feedback>()))
                .Callback<Feedback>(a => actualFeedback = a);

            ////Act
            this._sut.Create(feedback);

            ////Assert
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
        public void CreatePostAction_ModelIsValid_ArgumentExceptionThrown()
        {
            ////Arrange
            var feedback = new FeedbackMvcViewModelBuilder().Build();
            this._feedbackServiceMock.Setup(f => f.Create(It.IsAny<Feedback>()))
                .Throws(new ArgumentException());

            ////Act
            var sut = this._kernel.Get<FeedbacksController>();
            sut.Create(feedback);

            ////Assert
            Assert.IsTrue(this._sut.ModelState.IsValid);
        }

        /// <summary>
        /// Test for Feedbacks controller GetUsersEmailById method.
        /// If user is authenticated returns email.
        /// </summary>
        [TestMethod]
        public void GetUsersEmailByIdMethod_UserIsAuthentificated_UserEmailReturned()
        {
            ////Arrange
            var user = new UserMvcViewModelBuilder()
                .Build();
            var userDomain = new UserModelBuilder()
                .WithEmail(TEST_MAIL)
                .Build();

            this._userServiceMock.Setup(us => us.GetCurrentUserId())
                .Returns(user.Id);
            this._userStoreMock.Setup(um => um.FindByIdAsync(user.Id))
                .ReturnsAsync(userDomain);

            ////Act
            PrivateObject sut = new PrivateObject(this._sut);
            var userEmail = sut.Invoke("GetUsersEmailById", user.Id);

            ////Assert
            Assert.IsTrue(string.Equals(TEST_MAIL, (string)userEmail));
        }
    }
}
