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

    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.FeedbackAggregate;
    using VolleyManagement.UI.Areas.Admin.Controllers;
    using VolleyManagement.UI.Areas.Admin.Models;
    using VolleyManagement.UnitTests.Mvc.Comparers;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RequestsControllerTests
    {
        #region Fields

        private const int EXISTING_ID = 1;
        private const string ERROR_PAGE = "ErrorPage";
        private const string MESSAGE = "Test reply message";

        private Mock<IFeedbackService> _feedbacksServiceMock;


        #endregion

        #region Init

        [TestInitialize]
        public void TestInit()
        {
            _feedbacksServiceMock = new Mock<IFeedbackService>();
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

            var sut = BuildSut();

            // Act
            var actionResult = sut.Index();

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
            MockGetFeedback(FEEDBACK_ID, feedback);
            RequestsViewModel expected = new RequestsViewModel(feedback);

            var sut = BuildSut();

            // Act
            var actionResult = sut.Details(FEEDBACK_ID);

            // Assert
            var actual = TestExtensions.GetModel<RequestsViewModel>(actionResult);
            AreDetailsModelsEqual(expected, actual);
        }

        [TestMethod]
        public void Details_FeedbackDoesNotExist_HttpNotFoundResultIsReturned()
        {
            // Arrange
            MockGetFeedback(EXISTING_ID, null);

            var sut = BuildSut();

            // Act
            var result = sut.Details(EXISTING_ID);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Close_AnyFeedback_FeedbackRedirectToIndex()
        {
            // Arrange
            const int FEEDBACK_ID = 1;

            var sut = BuildSut();

            // Act
            var actionResult = sut.Close(FEEDBACK_ID);

            // Assert
            AssertValidRedirectResult(actionResult, "Index");
        }

        [TestMethod]
        public void Close_AnyFeedback_FeedbackRedirectToErrorPage()
        {
            // Arrange            
            SetupCloseThrowInvalidOperationException();
            var sut = BuildSut();

            // Act
            var actionResult = sut.Close(EXISTING_ID) as ViewResult;

            // Assert
            Assert.AreEqual(ERROR_PAGE, actionResult.ViewName);
        }

        [TestMethod]
        public void Close_FeedbackDoesNotExist_FeedbackRedirectToErrorPage()
        {
            // Arrange
            SetupCloseThrowMissingEntityException();

            var sut = BuildSut();

            // Act
            var actionResult = sut.Close(EXISTING_ID) as ViewResult;

            // Assert
            Assert.AreEqual(ERROR_PAGE, actionResult.ViewName);
        }

        [TestMethod]
        public void Close_AnyFeedback_FeedbackClosed()
        {
            // Arrange
            const int FEEDBACK_ID = 1;

            var sut = BuildSut();

            // Act
            var actionResult = sut.Close(FEEDBACK_ID);

            // Assert
            AssertCloseVerify(_feedbacksServiceMock, FEEDBACK_ID);
        }

        [TestMethod]
        public void Reply_AnyFeedback_FeedbackFromIndexRedirectToReply()
        {
            // Arrange
            const int FEEDBACK_ID = 1;
            var sut = BuildSut();

            // Act
            var actionResult = sut.Reply(FEEDBACK_ID, "message");

            // Assert
            AssertValidRedirectResult(actionResult, "Index");
        }

        [TestMethod]
        public void Reply_AnyFeedback_FeedbackReplied()
        {
            // Arrange
            const int FEEDBACK_ID = 1;
            var sut = BuildSut();

            // Act
            var actionResult = sut.Reply(FEEDBACK_ID, MESSAGE) as ViewResult;

            // Assert
            AssertReplyVerify(_feedbacksServiceMock, FEEDBACK_ID, MESSAGE);
        }

        [TestMethod]
        public void Reply_AnyFeedback_FeedbackRedirectToErrorPage()
        {
            // Arrange

            SetupReplyThrowInvalidOperationException();
            var sut = BuildSut();

            // Act
            var actionResult = sut.Reply(EXISTING_ID, "message") as ViewResult;

            // Assert
            Assert.AreEqual(ERROR_PAGE, actionResult.ViewName);
        }

        [TestMethod]
        public void Reply_FeedbackDoesNotExist_FeedbackRedirectToErrorPage()
        {
            // Arrange
            SetupReplyThrowMissingEntityException();
            var sut = BuildSut();

            // Act
            var actionResult = sut.Reply(EXISTING_ID, "message") as ViewResult;

            // Assert
            Assert.AreEqual(ERROR_PAGE, actionResult.ViewName);
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

        private static void AreDetailsModelsEqual(RequestsViewModel expected, RequestsViewModel actual)
        {
            Assert.AreEqual(expected.Id, actual.Id, "Requests ID does not match");
            Assert.AreEqual(expected.Content, actual.Content, "Requests Content are different");
            Assert.AreEqual(expected.AdminName, actual.AdminName, "Requests AdminName are different");
            Assert.AreEqual(expected.Date, actual.Date, "Requests Date are different");
            Assert.AreEqual(expected.Status, actual.Status, "Requests Status are different");
            Assert.AreEqual(expected.UpdateDate, actual.UpdateDate, "Requests UpdateDate are different");
            Assert.AreEqual(expected.UsersEmail, actual.UsersEmail, "Requests UsersEmail are different");
        }

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

        private static void AssertReplyVerify(Mock<IFeedbackService> mock, int feedbackId, string feedbackMessage)
        {
            mock.Verify(
                ps => ps.Reply(
                It.Is<int>(id => id == feedbackId),
                It.Is<string>(message => message == feedbackMessage)),
                Times.Once());
        }

        #endregion

        #region Mock
        private RequestsController BuildSut()
        {
            return new RequestsController(_feedbacksServiceMock.Object);
        }

        private void MockGetFeedback(int feedbackID, Feedback feedback)
        {
            _feedbacksServiceMock.Setup(r => r.GetDetails(feedbackID)).Returns(feedback);
        }

        private void SetupCloseThrowInvalidOperationException()
        {
            _feedbacksServiceMock.Setup(ts => ts.Close(It.IsAny<int>()))
                .Throws(new InvalidOperationException(string.Empty));
        }

        private void SetupCloseThrowMissingEntityException()
        {
            _feedbacksServiceMock.Setup(ts => ts.Close(It.IsAny<int>()))
                .Throws(new MissingEntityException(string.Empty));
        }

        private void SetupReplyThrowInvalidOperationException()
        {
            _feedbacksServiceMock.Setup(ts => ts.Reply(It.IsAny<int>(), It.IsAny<string>()))
                .Throws(new InvalidOperationException(string.Empty));
        }

        private void SetupReplyThrowMissingEntityException()
        {
            _feedbacksServiceMock.Setup(ts => ts.Reply(It.IsAny<int>(), It.IsAny<string>()))
                .Throws(new MissingEntityException(string.Empty));
        }
        #endregion
    }
}