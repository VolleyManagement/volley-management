namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    using Contracts;
    using Domain.RolesAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Ninject;

    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Domain.Dto;
    using VolleyManagement.Domain.FeedbackAggregate;
    using VolleyManagement.UI.Areas.Admin.Controllers;
    using VolleyManagement.UI.Areas.Admin.Models;
    using VolleyManagement.UnitTests.Mvc.Comparers;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RequestsControllerTests
    {
        #region Fields

        private IKernel _kernel;

        private Mock<IFeedbackService> _feedbacksServiceMock;

        private Mock<IAuthorizationService> _authServiceMock = new Mock<IAuthorizationService>();

        #endregion

        #region Init

        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();

            _kernel.RegisterDefaultMock(out _feedbacksServiceMock);

            _kernel.Bind<IAuthorizationService>().ToConstant(_authServiceMock.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void Index_NewFeedbacks_AllFeedbacksReturned()
        {
            // Arrange
            var feedbacks = GetNewFeedbacks();
            var expected = GetNewRequestsViewModels();
            _feedbacksServiceMock.Setup(r => r.Get()).Returns(feedbacks);
            var controller = _kernel.Get<RequestsController>();

            // Act
            var actionResult = controller.Index();

            // Assert
            var actual = GetModel<List<RequestsViewModel>>(actionResult);
            CollectionAssert.AreEqual(expected, actual, new RequestsViewModelComparer());

            VerifyCheckAccess(AuthOperations.AdminDashboard.View, Times.Once());
        }

        [TestMethod]
        public void Details_FeedbackWithReplies_DetailsModelReturned()
        {
            // Arrange
            const int FEEDBACK_ID = 1;
            var feedback = GetAnyFeedback(FEEDBACK_ID);
            MockGetFeedbacks(FEEDBACK_ID, feedback);
            RequestsViewModel expected = new RequestsViewModel(feedback);
            var controller = _kernel.Get<RequestsController>();
            //// Act
            var actionResult = controller.Details(FEEDBACK_ID);
            //// Assert
            var actual = GetModel<Feedback>(actionResult);
            RequestsViewModel act = new RequestsViewModel(actual);
            AssertAreDetailsModelsEqual(expected, act);

            VerifyCheckAccess(AuthOperations.AdminDashboard.View, Times.Once());
        }

        [TestMethod]
        public void Close_AnyFeedback_FeedbackRedirectToIndex()
        {
            // Arrange
            const int FEEDBACK_ID = 1;
            var service = _kernel.Get<RequestsController>();
            //// Act
            var actionResult = service.Close(FEEDBACK_ID);
            AssertValidRedirectResult(actionResult);

            VerifyCheckAccess(AuthOperations.AdminDashboard.View, Times.Once());
        }

        [TestMethod]
        public void Close_NotClosedFeedback_FeedbackDetails()
        {
            // Arrange
            const int FEEDBACK_ID = 1;
            var feedback = GetNotClosedFeedback(FEEDBACK_ID);
            MockGetFeedbacks(FEEDBACK_ID, feedback);
            RequestsViewModel expected = new RequestsViewModel(feedback);

            var controller = _kernel.Get<RequestsController>();
            //// Act
            var actionCloseResult = controller.Close(FEEDBACK_ID);
            //// Assert
            var actionDetailsResult = controller.Details(FEEDBACK_ID);
            //// Assert
            var actual = GetModel<Feedback>(actionDetailsResult);
            RequestsViewModel act = new RequestsViewModel(actual);
            AssertAreDetailsModelsEqual(expected, act);

            VerifyCheckAccess(AuthOperations.AdminDashboard.View, Times.Exactly(2));
        }

        [TestMethod]
        public void Reply_AnyFeedback_FeedbackReply()
        {
            // Arrange
            const int FEEDBACK_ID = 1;
            var feedback = GetNotClosedFeedback(FEEDBACK_ID);
            _feedbacksServiceMock.Setup(r => r.GetDetails(FEEDBACK_ID)).Returns(feedback);
            RequestsViewModel expected = new RequestsViewModel(feedback);

            var controller = _kernel.Get<RequestsController>();
            //// Act
            var actionCloseResult = controller.Reply(FEEDBACK_ID);
            //// Assert
            var actionDetailsResult = controller.Details(FEEDBACK_ID);
            //// Assert
            var actual = GetModel<Feedback>(actionDetailsResult);
            RequestsViewModel act = new RequestsViewModel(actual);
            AssertAreDetailsModelsEqual(expected, act);

            VerifyCheckAccess(AuthOperations.AdminDashboard.View, Times.Exactly(2));
        }
        #endregion

        #region Test Data

        private static List<Feedback> GetNewFeedbacks()
        {
            return new List<Feedback>
                       {
                           new Feedback { Id = 1, Content = "Content1", UsersEmail = "1@gmail.com", Date = new DateTime(2016, 10, 25) },
                           new Feedback { Id = 2, Content = "Content2", UsersEmail = "2@gmail.com", Date = new DateTime(2016, 10, 24) },
                       };
        }

        private static List<RequestsViewModel> GetNewRequestsViewModels()
        {
            return new List<RequestsViewModel>
                       {
                           new RequestsViewModel
                           {
                               Id = 1,
                               Content = "Content1",
                               UsersEmail = "1@gmail.com",
                               Date = new DateTime(2016, 10, 25)
                           },
                           new RequestsViewModel
                           {
                               Id = 2,
                               Content = "Content2",
                               UsersEmail = "2@gmail.com",
                               Date = new DateTime(2016, 10, 24)
                           },
                        };
        }

        private static Feedback GetAnyFeedback(int feedbackId)
        {
            return new Feedback
            {
                Id = feedbackId,
                Content = "Content2",
                UsersEmail = "2@gmail.com",
                Status = FeedbackStatusEnum.Read,
                AdminName = "Admin2",
                UpdateDate = new DateTime(2016, 10, 25),
                Date = new DateTime(2016, 10, 24)
            };
        }

        private static Feedback GetNotClosedFeedback(int feedbackId)
        {
            return new Feedback
            {
                Id = feedbackId,
                Content = "Content2",
                UsersEmail = "2@gmail.com",
                Status = FeedbackStatusEnum.Answered,
                AdminName = "Admin2",
                UpdateDate = new DateTime(2016, 10, 25),
                Date = new DateTime(2016, 10, 24)
            };
        }

        #endregion

        #region Custom assertions

        private static void AssertAreDetailsModelsEqual(RequestsViewModel expected, RequestsViewModel actual)
        {
            Assert.AreEqual(expected.Id, actual.Id, "Feedback ID does not match");
            Assert.AreEqual(expected.AdminName, actual.AdminName, "Feedback AdminNames are different");
            Assert.AreEqual(expected.Content, actual.Content, "Feedback Content are different");
            Assert.AreEqual(expected.Date, actual.Date, "Feedback Date are different");
            Assert.AreEqual(expected.UsersEmail, actual.UsersEmail, "Feedback Email are different");
            Assert.AreEqual(expected.Status, actual.Status, "Feedback Status are different");
            Assert.AreEqual(expected.UpdateDate, actual.UpdateDate, "Feedback UpdateDate are different");
            //// Assert.AreEqual(actual.UserEnvironment, expected.UserEnvironment, "Feedback UserEnvironment are different");
        }

        private static void AssertValidRedirectResult(ActionResult actionResult)
        {
            var result = (RedirectToRouteResult)actionResult;
            Assert.IsFalse(result.Permanent, "Redirect should not be permanent");
            Assert.AreEqual(1, result.RouteValues.Count, "Redirect should forward to Requests.Index action");
            Assert.AreEqual("Index", result.RouteValues["action"], "Redirect should forward to Requests.Index action");
        }
        #endregion

        #region Helpers

        private static T GetModel<T>(ActionResult actionResult)
        {
            return (T)((ViewResult)actionResult).Model;
        }
        #endregion

        #region Mock
        private void MockGetFeedbacks(int feedbackID, Feedback feedback)
        {
            _feedbacksServiceMock.Setup(r => r.GetDetails(feedbackID)).Returns(feedback);
        }
        #endregion

        #region Private

        private void VerifyCheckAccess(AuthOperation operation, Times times)
        {
            _authServiceMock.Verify(tr => tr.CheckAccess(operation), times);
        }

        #endregion
    }
}