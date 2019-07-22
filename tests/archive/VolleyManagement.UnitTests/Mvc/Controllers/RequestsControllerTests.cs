namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Contracts;
    using Xunit;
    using Moq;
    using Contracts.Exceptions;
    using Domain.FeedbackAggregate;
    using FluentAssertions;
    using UI.Areas.Admin.Controllers;
    using UI.Areas.Admin.Models;
    using Comparers;

    [ExcludeFromCodeCoverage]
    public class RequestsControllerTests
    {
        #region Fields

        private const int EXISTING_ID = 1;
        private const string ERROR_PAGE = "ErrorPage";
        private const string MESSAGE = "Test reply message";

        private Mock<IFeedbackService> _feedbacksServiceMock;

        #endregion

        #region Init

        public RequestsControllerTests()
        {
            _feedbacksServiceMock = new Mock<IFeedbackService>();
        }

        #endregion

        #region Tests

        [Fact]
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
            Assert.Equal(expected, actual, new RequestsViewModelComparer());
        }

        [Fact]
        public void Details_FeedbackWithReplies_DetailsModelReturned()
        {
            // Arrange
            const int FEEDBACK_ID = 1;
            var feedback = GetAnyFeedback(FEEDBACK_ID);
            MockGetFeedback(FEEDBACK_ID, feedback);
            var expected = new RequestsViewModel(feedback);

            var sut = BuildSut();

            // Act
            var actionResult = sut.Details(FEEDBACK_ID);

            // Assert
            var actual = TestExtensions.GetModel<RequestsViewModel>(actionResult);
            AreDetailsModelsEqual(expected, actual);
        }

        [Fact]
        public void Details_FeedbackDoesNotExist_HttpNotFoundResultIsReturned()
        {
            // Arrange
            MockGetFeedback(EXISTING_ID, null);

            var sut = BuildSut();

            // Act
            var result = sut.Details(EXISTING_ID);

            // Assert
            Assert.IsType<HttpNotFoundResult>(result);
        }

        [Fact]
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

        [Fact]
        public void Close_AnyFeedback_FeedbackRedirectToErrorPage()
        {
            // Arrange
            SetupCloseThrowInvalidOperationException();
            var sut = BuildSut();

            // Act
            var actionResult = sut.Close(EXISTING_ID) as ViewResult;

            // Assert
            Assert.Equal(ERROR_PAGE, actionResult.ViewName);
        }

        [Fact]
        public void Close_FeedbackDoesNotExist_FeedbackRedirectToErrorPage()
        {
            // Arrange
            SetupCloseThrowMissingEntityException();

            var sut = BuildSut();

            // Act
            var actionResult = sut.Close(EXISTING_ID) as ViewResult;

            // Assert
            Assert.Equal(ERROR_PAGE, actionResult.ViewName);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
        public void Reply_AnyFeedback_FeedbackRedirectToErrorPage()
        {
            // Arrange
            SetupReplyThrowInvalidOperationException();
            var sut = BuildSut();

            // Act
            var actionResult = sut.Reply(EXISTING_ID, "message") as ViewResult;

            // Assert
            Assert.Equal(ERROR_PAGE, actionResult.ViewName);
        }

        [Fact]
        public void Reply_FeedbackDoesNotExist_FeedbackRedirectToErrorPage()
        {
            // Arrange
            SetupReplyThrowMissingEntityException();
            var sut = BuildSut();

            // Act
            var actionResult = sut.Reply(EXISTING_ID, "message") as ViewResult;

            // Assert
            Assert.Equal(ERROR_PAGE, actionResult.ViewName);
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
            return new Feedback {
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
            return new Feedback {
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
            actual.Id.Should().Be(expected.Id, "Requests ID does not match");
            actual.Content.Should().Be(expected.Content, "Requests Content are different");
            actual.AdminName.Should().Be(expected.AdminName, "Requests AdminName are different");
            actual.Date.Should().Be(expected.Date, "Requests Date are different");
            actual.Status.Should().Be(expected.Status, "Requests Status are different");
            actual.UpdateDate.Should().Be(expected.UpdateDate, "Requests UpdateDate are different");
            actual.UsersEmail.Should().Be(expected.UsersEmail, "Requests UsersEmail are different");
        }

        private static void AssertValidRedirectResult(ActionResult actionResult, string view)
        {
            var result = (RedirectToRouteResult)actionResult;
            Assert.False(result.Permanent, "Redirect should not be permanent");
            result.RouteValues.Count.Should().Be(1, $"Redirect should forward to Requests.{view} action");
            result.RouteValues["action"].Should().Be(view, $"Redirect should forward to Requests.{view} action");
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