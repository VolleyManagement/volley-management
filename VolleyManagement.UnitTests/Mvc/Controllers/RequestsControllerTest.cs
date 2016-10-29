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

        private Mock<IFeedbackService> _requestsServiceMock;

        private Mock<IAuthorizationService> _authServiceMock = new Mock<IAuthorizationService>();

        #endregion

        #region Init

        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();

            _kernel.RegisterDefaultMock(out _requestsServiceMock);

            _kernel.Bind<IAuthorizationService>().ToConstant(_authServiceMock.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void Index_DefaultRequests_AllRequestsReturned()
        {
            // Arrange
            var requests = GetDefaultFeedbacks();
            var expected = GetDefaultRequestViewModels();
            _requestsServiceMock.Setup(r => r.Get()).Returns(requests);
            var service = _kernel.Get<RequestsController>();

            // Act
            var actionResult = service.Index();

            // Assert
            var actual = GetModel<List<RequestsViewModel>>(actionResult);
            CollectionAssert.AreEqual(expected, actual, new RequestsViewModelComparer());

            VerifyCheckAccess(AuthOperations.AdminDashboard.View, Times.Once());
        }

        [TestMethod]
        public void Details_RequestWithReplies_DetailsModelReturned()
        {
            // Arrange
            const int FEEDBACK_ID = 1;
            var request = GetAnyRequest(FEEDBACK_ID);
            _requestsServiceMock.Setup(r => r.GetDetails(FEEDBACK_ID)).Returns(request);
            RequestsViewModel expected = new RequestsViewModel(request);
            var service = _kernel.Get<RequestsController>();
            // Act
            var actionResult = service.Details(FEEDBACK_ID);
            // Assert
            var actual = GetModel<Feedback>(actionResult);
            RequestsViewModel act = new RequestsViewModel(actual);
             AreDetailsModelsEqual(expected, act);

            VerifyCheckAccess(AuthOperations.AdminDashboard.View, Times.Once());
        }

        [TestMethod]
        public void Close_AnyRequest_RequestRedirectToIndex()
        {
            // Arrange
            const int FEEDBACK_ID = 1;
            var service = _kernel.Get<RequestsController>();
            // Act
            var actionResult = service.Close(FEEDBACK_ID);
            AssertValidRedirectResult(actionResult);

            VerifyCheckAccess(AuthOperations.AdminDashboard.View, Times.Once());
        }

        [TestMethod]
        public void Close_NotClosedRequest_RequestClosed()
        {
            // Arrange
            const int FEEDBACK_ID = 1;
            var request = GetNotClosedFeedback(FEEDBACK_ID);
            _requestsServiceMock.Setup(r => r.GetDetails(FEEDBACK_ID)).Returns(request);
            RequestsViewModel expected = new RequestsViewModel(request);

            var service = _kernel.Get<RequestsController>();
            // Act
            var actionCloseResult = service.Close(FEEDBACK_ID);
            // Assert
            var actionDetailsResult = service.Details(FEEDBACK_ID);
            // Assert
            var actual = GetModel<Feedback>(actionDetailsResult);
            RequestsViewModel act = new RequestsViewModel(actual);
            AreDetailsModelsEqual(expected, act);

            VerifyCheckAccess(AuthOperations.AdminDashboard.View, Times.Exactly(2));
        }
        [TestMethod]
        public void Index_AnyRequest_RequestReply()
        {
            // Arrange
            const int FEEDBACK_ID = 1;
            var request = GetNotClosedFeedback(FEEDBACK_ID);
            _requestsServiceMock.Setup(r => r.GetDetails(FEEDBACK_ID)).Returns(request);
            RequestsViewModel expected = new RequestsViewModel(request);

            var service = _kernel.Get<RequestsController>();
            // Act
            var actionCloseResult = service.Reply(FEEDBACK_ID);
            // Assert
            var actionDetailsResult = service.Details(FEEDBACK_ID);
            // Assert
            var actual = GetModel<Feedback>(actionDetailsResult);
            RequestsViewModel act = new RequestsViewModel(actual);
            AreDetailsModelsEqual(expected, act);

            VerifyCheckAccess(AuthOperations.AdminDashboard.View, Times.Exactly(2));
        }
        #endregion

        #region Test Data

        private static List<Feedback> GetDefaultFeedbacks()
        {
            return new List<Feedback>
                       {
                           new Feedback { Id = 1,Content = "Content1",UsersEmail="1@gmail.com", Date=new DateTime(2016,10,25) },
                           new Feedback { Id = 2,Content = "Content2",UsersEmail="2@gmail.com", Date=new DateTime(2016,10,24)},
                       };
        }

        private static List<RequestsViewModel> GetDefaultRequestViewModels()
        {
            return new List<RequestsViewModel>
                       {
                           new RequestsViewModel { Id = 1,Content = "Content1",UsersEmail="1@gmail.com", Date=new DateTime(2016,10,25) },
                           new RequestsViewModel { Id = 2,Content = "Content2",UsersEmail="2@gmail.com", Date=new DateTime(2016,10,24)},
                        };
        }

        private static Feedback GetAnyRequest(int requestId)
        {
            return new Feedback { Id = requestId, Content = "Content2", UsersEmail = "2@gmail.com", Status = FeedbackStatusEnum.Read, AdminName = "Admin2", UpdateDate = new DateTime(2016, 10, 25), Date = new DateTime(2016, 10, 24) };
        }

        private static Feedback GetNotClosedFeedback(int requestId)
        {
            return new Feedback { Id = requestId, Content = "Content2", UsersEmail = "2@gmail.com", Status = FeedbackStatusEnum.Answered, AdminName = "Admin2", UpdateDate = new DateTime(2016, 10, 25), Date = new DateTime(2016, 10, 24) };
        }
        
        #endregion

        #region Custom assertions

        private static void AreDetailsModelsEqual(RequestsViewModel expected, RequestsViewModel actual)
        {
            Assert.AreEqual(expected.Id, actual.Id, "Request ID does not match");
            Assert.AreEqual(expected.AdminName, actual.AdminName, "Request AdminNames are different");
            Assert.AreEqual(expected.Content, actual.Content, "Request Content are different");
            Assert.AreEqual(expected.Date, actual.Date, "Request Date are different");
            Assert.AreEqual(expected.UsersEmail, actual.UsersEmail, "Request Email are different");
            Assert.AreEqual(expected.Status, actual.Status, "Request Status are different");
            Assert.AreEqual(expected.UpdateDate, actual.UpdateDate, "Request UpdateDate are different");
            //    Assert.AreEqual(actual.UserEnvironment, expected.UserEnvironment, "Request UserEnvironment are different");
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

        //public T ModelFromActionResult<T>(RedirectToRouteResult actionResult)
        //{
        //    object model;
        //    if (actionResult.GetType() == typeof(ViewResult))
        //    {
        //        ViewResult viewResult = (ViewResult)actionResult;
        //        model = viewResult.Model;
        //    }
        //    else if (actionResult.GetType() == typeof(PartialViewResult))
        //    {
        //        PartialViewResult partialViewResult = (PartialViewResult)actionResult;
        //        model = partialViewResult.Model;
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException(string.Format("Actionresult of type {0} is not supported by ModelFromResult extractor.", actionResult.GetType()));
        //    }
        //    T typedModel = (T)model;
        //    return typedModel;
        //}
        #endregion

        #region Private

        private void VerifyCheckAccess(AuthOperation operation, Times times)
        {
            _authServiceMock.Verify(tr => tr.CheckAccess(operation), times);
        }

        #endregion
    }
}