namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Web.Mvc;

    using Contracts;
    using Contracts.Authentication;
    using Contracts.Authentication.Models;
    using Domain.FeedbackAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using Services.FeedbackService;
    using UI.Areas.Mvc.Controllers;
    using ViewModels;

    [TestClass]
    public class FeedbacksControllerTests
    {
        private readonly Mock<IFeedbackService> _feedbackServiceMock =
            new Mock<IFeedbackService>();

        private readonly Mock<IVolleyUserManager<UserModel>> _userManagerMock =
            new Mock<IVolleyUserManager<UserModel>>();

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
            this._kernel.Bind<IVolleyUserManager<UserModel>>()
                .ToConstant(this._userManagerMock.Object);
            this._sut = this._kernel.Get<FeedbacksController>();
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
            Assert.AreEqual("Create", result.ViewName);
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
            Assert.AreEqual("FeedbackSentMessage", result.ViewName);
        }

        /// <summary>
        /// Test for Create POST method. Valid model passed. Feedback created.
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
        /// Test for Create POST method. While calling IFeedbackService method
        /// Create() argument exception is thrown.
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
    }
}
