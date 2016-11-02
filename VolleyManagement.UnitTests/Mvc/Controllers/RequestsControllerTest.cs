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
        #endregion

        #region Init

        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();

            _kernel.RegisterDefaultMock(out _feedbacksServiceMock);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void Index_FeedbacksExist_AllFeedbacksReturned()
        {
            // Arrange
            var feedbacks = GetFeedbacks();
            var expected = GetRequestsViewModels();
            _feedbacksServiceMock.Setup(r => r.Get()).Returns(feedbacks);
            var controller = _kernel.Get<RequestsController>();

            // Act
            var actionResult = controller.Index();

            // Assert
            var actual = TestExtensions.GetModel<List<RequestsViewModel>>(actionResult);
            CollectionAssert.AreEqual(expected, actual, new RequestsViewModelComparer());
        }

        [TestMethod]
        public void Details_FeedbackWithReplies_DetailsModelReturned()
        {
            // Arrange
            const int FEEDBACK_ID = 1;
            var feedback = GetAnyFeedback(FEEDBACK_ID);
            MockGetFeedbacks(FEEDBACK_ID, feedback);
            Feedback expected = feedback;
            var controller = _kernel.Get<RequestsController>();

            // Act
            var actionResult = controller.Details(FEEDBACK_ID);

            // Assert
            var actual = TestExtensions.GetModel<Feedback>(actionResult);
            Assert.AreEqual<Feedback>(expected, actual);
        }

        [TestMethod]
        public void Close_AnyFeedback_FeedbackRedirectToIndex()
        {
            // Arrange
            const int FEEDBACK_ID = 1;
            var controller = _kernel.Get<RequestsController>();

            // Act
            var actionResult = controller.Close(FEEDBACK_ID);

            // Assert
            AssertValidRedirectResult(actionResult, "Index");
        }

        [TestMethod]
        public void Close_AnyFeedback_FeedbackClosed()
        {
            // Arrange
            const int FEEDBACK_ID = 1;
            var controller = _kernel.Get<RequestsController>();

            // Act
            var actionResult = controller.Close(FEEDBACK_ID);

            // Assert
            AssertCloseVerify(_feedbacksServiceMock, FEEDBACK_ID);
        }

        [TestMethod]
        public void Reply_AnyFeedback_FeedbackFromIndexRedirectToReply()
        {
            // Arrange
            const int FEEDBACK_ID = 1;
            var controller = _kernel.Get<RequestsController>();

            // Act
            var actionResult = controller.Reply(FEEDBACK_ID);

            // Assert
            AssertValidRedirectResult(actionResult, "Index");
        }

        [TestMethod]
        public void Reply_AnyFeedback_FeedbackReplied()
        {
            // Arrange
            const int FEEDBACK_ID = 1;
            var controller = _kernel.Get<RequestsController>();

            // Act
            var actionResult = controller.Reply(FEEDBACK_ID);

            // Assert
            AssertReplyVerify(_feedbacksServiceMock, FEEDBACK_ID);
        }
        #endregion

        #region Test Data

        private static List<Feedback> GetFeedbacks()
        {
            return new List<Feedback>
                       {
                           new Feedback
                           {
                               Id = 1,
                               Content = "Content1",
                               UsersEmail = "1@gmail.com",
                               Date = new DateTime(2016, 10, 25)
                           },
                           new Feedback
                           {
                               Id = 2,
                               Content = "Content2",
                               UsersEmail = "2@gmail.com",
                               Status = FeedbackStatusEnum.Read,
                               Date = new DateTime(2016, 10, 24)
                           },
                       };
        }

        private static List<RequestsViewModel> GetRequestsViewModels()
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
                               Date = new DateTime(2016, 10, 24),
                               Status = FeedbackStatusEnum.Read
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

        private static void AssertValidRedirectResult(ActionResult actionResult, string view)
        {
            var result = (RedirectToRouteResult)actionResult;
            Assert.IsFalse(result.Permanent, "Redirect should not be permanent");
            Assert.AreEqual(1, result.RouteValues.Count, string.Format("Redirect should forward to Requests.{0} action", view));
            Assert.AreEqual(view, result.RouteValues["action"], string.Format("Redirect should forward to Requests.{0} action", view));
        }

        private static void AssertCloseVerify(Mock<IFeedbackService> mock, int feedbackId)
        {
            mock.Verify(ps => ps.Close(It.Is<int>(id => id == feedbackId)), Times.Once());
        }

        private static void AssertReplyVerify(Mock<IFeedbackService> mock, int feedbackId)
        {
            mock.Verify(ps => ps.Reply(It.Is<int>(id => id == feedbackId)), Times.Once());
        }
        #endregion

        #region Mock
        private void MockGetFeedbacks(int feedbackID, Feedback feedback)
        {
            _feedbacksServiceMock.Setup(r => r.GetDetails(feedbackID)).Returns(feedback);
        }
        #endregion
    }
}