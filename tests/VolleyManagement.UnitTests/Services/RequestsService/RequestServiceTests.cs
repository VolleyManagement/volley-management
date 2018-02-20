using System.Collections;

namespace VolleyManagement.UnitTests.Services.RequestsService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MSTestExtensions;

    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.Player;
    using VolleyManagement.Domain.RequestsAggregate;
    using VolleyManagement.Domain.RolesAggregate;
    using VolleyManagement.Domain.UsersAggregate;
    using VolleyManagement.Services;
    using VolleyManagement.UnitTests.Services.UsersService;

    /// <summary>
    /// Tests for RequestService class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RequestServiceTests : BaseTest
    {
        #region Fields and constants

        private const int EXISTING_ID = 1;
        private const int EXISTING_USER_ID = 1;
        private const int EXISTING_PLAYER_ID = 1;
        private const int INVALID_REQUEST_ID = -1;
        private const int INVALID_USER_ID = -1;
        private const int INVALID_PLAYER_ID = -1;

        private RequestServiceTestFixture _testFixture = new RequestServiceTestFixture();

        private Mock<IUserService> _userServiceMock;
        private Mock<IRequestRepository> _requestRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IAuthorizationService> _authServiceMock;
        private Mock<IQuery<Request, FindByIdCriteria>> _getRequestByIdQueryMock;
        private Mock<IQuery<User, FindByIdCriteria>> _getUserByIdQueryMock;
        private Mock<IQuery<ICollection<Request>, GetAllCriteria>> _getAllRequestsQueryMock;
        private Mock<IQuery<Request, UserToPlayerCriteria>> _getRequestUserPlayerQueryMock;

        private Mock<IUnitOfWork> _unitOfWorkMock;

        #endregion

        #region Constuctor

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _userServiceMock = new Mock<IUserService>();
            _requestRepositoryMock = new Mock<IRequestRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _authServiceMock = new Mock<IAuthorizationService>();
            _getRequestByIdQueryMock = new Mock<IQuery<Request, FindByIdCriteria>>();
            _getUserByIdQueryMock = new Mock<IQuery<User, FindByIdCriteria>>();
            _getAllRequestsQueryMock = new Mock<IQuery<ICollection<Request>, GetAllCriteria>>();
            _getRequestUserPlayerQueryMock = new Mock<IQuery<Request, UserToPlayerCriteria>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _requestRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);
            _userRepositoryMock.Setup(pr => pr.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }

        #endregion

        #region Request tests
        [TestMethod]
        public void GetAll_RequestsExist_RequestsReturned()
        {
            // Arrange
            var expected = _testFixture.TestRequests().Build();
            MockGetAllRequestsQuery(expected);

            // Act
            var sut = BuildSUT();
            var actual = sut.Get();

            // Assert
            CollectionAssert.AreEqual(expected, actual as ICollection, new RequestComparer());
        }

        [TestMethod]
        public void GetAll_NoViewListRights_AuthorizationExceptionThrown()
        {
            // Arrange
            MockAuthServiceThrowsException(AuthOperations.Requests.ViewList);

            var sut = BuildSUT();

            // Act => Assert
            Assert.Throws<AuthorizationException>(() => sut.Get(), "Requested operation is not allowed");
        }

        [TestMethod]
        public void GetById_RequestExists_RequestReturned()
        {
            // Arrange
            var expected = new RequestBuilder().WithId(EXISTING_ID).Build();
            MockGetRequestByIdQuery(expected);

            var sut = BuildSUT();

            // Act
            var actual = sut.Get(EXISTING_ID);

            // Assert
            TestHelper.AreEqual<Request>(expected, actual, new RequestComparer());
        }

        [TestMethod]
        public void Create_InvalidUserId_ExceptionThrown()
        {
            // Arrange
            var sut = BuildSUT();

            // Act => Assert
            Assert.Throws<ArgumentException>(() => sut.Create(INVALID_REQUEST_ID, EXISTING_ID), "User's id is wrong");
        }

        [TestMethod]
        public void Create_InvalidPlayerId_ExceptionThrown()
        {
            // Arrange
            var sut = BuildSUT();

            // Act => Assert
            Assert.Throws<ArgumentException>(() => sut.Create(EXISTING_USER_ID, INVALID_PLAYER_ID), "Player's id is wrong");
        }

        [TestMethod]
        public void Create_ValidRequest_RequestAdded()
        {
            // Arrange
            var newRequest = new RequestBuilder()
                .WithUserId(EXISTING_USER_ID)
                .WithPlayerId(EXISTING_PLAYER_ID)
                .Build();

            _requestRepositoryMock.Setup(tr => tr.Add(It.IsAny<Request>()))
               .Callback<Request>(t => t.Id = EXISTING_ID);

            var sut = BuildSUT();

            // Act
            sut.Create(EXISTING_USER_ID, EXISTING_PLAYER_ID);

            // Assert
            VerifyCreateRequest(newRequest, Times.Once(), "Parameter request is not equal to Instance of request");
        }

        [TestMethod]
        public void Confirm_NoConfirmRights_DbNotChanged()
        {
            // Arrange
            Exception exception = null;
            var user = new UserBuilder().WithId(EXISTING_USER_ID).Build();
            MockAuthServiceThrowsException(AuthOperations.Requests.Confirm);

            var sut = BuildSUT();

            // Act
            try
            {
                sut.Confirm(EXISTING_ID);
            }
            catch (AuthorizationException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyEditUser(user, Times.Never());
            VerifyCheckAccess(AuthOperations.Requests.Confirm, Times.Once());
        }

        [TestMethod]
        public void Confirm_NoConfirmRights_AuthorizationExceptionThrown()
        {
            // Arrange
            var user = new UserBuilder().WithId(EXISTING_USER_ID).Build();
            MockAuthServiceThrowsException(AuthOperations.Requests.Confirm);

            var sut = BuildSUT();

            // Act => Assert
            Assert.Throws<AuthorizationException>(() => sut.Confirm(EXISTING_ID), "Requested operation is not allowed");
        }

        [TestMethod]
        public void Confirm_RequestDoesNotExist_ExceptionThrown()
        {
            // Arrange
            var sut = BuildSUT();

            // Act => Assert
            Assert.Throws<MissingEntityException>(
                () =>
                sut.Confirm(INVALID_REQUEST_ID),
                "A request with specified identifier was not found");
        }

        [TestMethod]
        public void Confirm_RequestExists_UserUpdated()
        {
            // Arrange
            var expected = new RequestBuilder().WithId(EXISTING_ID).Build();
            MockGetRequestByIdQuery(expected);
            var user = new UserBuilder().WithId(EXISTING_USER_ID).Build();
            MockGetUserByIdQuery(EXISTING_ID, user);
            var sut = BuildSUT();

            // Act
            sut.Confirm(EXISTING_ID);

            // Assert
            VerifyEditUser(user, Times.Once());
        }

        [TestMethod]
        public void Confirm_RequestExists_RequestDeleted()
        {
            // Arrange
            var expected = new RequestBuilder().WithId(EXISTING_ID).Build();
            MockGetRequestByIdQuery(expected);
            var user = new UserBuilder().WithId(EXISTING_USER_ID).Build();
            MockGetUserByIdQuery(EXISTING_USER_ID, user);
            var sut = BuildSUT();

            // Act
            sut.Confirm(EXISTING_ID);

            // Assert
            VerifyDeleteRequest(EXISTING_ID, Times.Once());
        }

        [TestMethod]
        public void Confirm_UserDoesNotExist_ExceptionThrown()
        {
            // Arrange
            var expected = new RequestBuilder().WithId(EXISTING_ID).Build();
            MockGetRequestByIdQuery(expected);
            MockGetUserByIdQuery(INVALID_USER_ID, null);
            var sut = BuildSUT();

            // Act => Assert
            Assert.Throws<MissingEntityException>(
                () =>
                sut.Confirm(EXISTING_ID),
                "A user with specified identifier was not found");
        }

        [TestMethod]
        public void Decline_NoDeclineRights_AuthorizationExceptionThrown()
        {
            // Arrange
            var user = new UserBuilder().WithId(EXISTING_USER_ID).Build();
            MockAuthServiceThrowsException(AuthOperations.Requests.Decline);

            var sut = BuildSUT();

            // Act => Assert
            Assert.Throws<AuthorizationException>(() => sut.Decline(EXISTING_ID), "Requested operation is not allowed");
        }

        [TestMethod]
        public void Decline_NoDeclineRights_DbNotChanged()
        {
            // Arrange
            Exception exception = null;
            MockAuthServiceThrowsException(AuthOperations.Requests.Decline);

            var sut = BuildSUT();

            // Act
            try
            {
                sut.Decline(EXISTING_ID);
            }
            catch (AuthorizationException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyDeleteRequest(EXISTING_ID, Times.Never());
            VerifyCheckAccess(AuthOperations.Requests.Decline, Times.Once());
        }

        [TestMethod]
        public void Decline_RequestExists_RequestDeleted()
        {
            // Arrange
            var sut = BuildSUT();

            // Act
            sut.Decline(EXISTING_ID);

            // Assert
            VerifyDeleteRequest(EXISTING_ID, Times.Once());
        }

        [TestMethod]
        public void Decline_RequestDoesNotExist_ExceptionThrown()
        {
            // Arrange
            MockRequestServiceThrowsInvalidKeyValueException();
            var sut = BuildSUT();

            // Act => Assert
            Assert.Throws<MissingEntityException>(
            () =>
            sut.Decline(INVALID_REQUEST_ID),
            "A request with specified identifier was not found");
        }

        [TestMethod]
        public void Decline_RequestDoesNotExist_DbNotChanged()
        {
            // Arrange
            MockRequestServiceThrowsInvalidKeyValueException();
            Exception exception = null;
            var sut = BuildSUT();

            // Act
            try
            {
                sut.Decline(INVALID_REQUEST_ID);
            }
            catch (MissingEntityException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyDeleteRequest(INVALID_REQUEST_ID, Times.Once(), Times.Never());
        }
        #endregion

        #region Private
        private RequestService BuildSUT()
        {
            return new RequestService(
                _requestRepositoryMock.Object,
                _userRepositoryMock.Object,
                _userServiceMock.Object,
                _authServiceMock.Object,
                _getRequestByIdQueryMock.Object,
                _getAllRequestsQueryMock.Object,
                _getRequestUserPlayerQueryMock.Object);
        }

        private void MockGetAllRequestsQuery(IEnumerable<Request> testData)
        {
            _getAllRequestsQueryMock.Setup(tr => tr.Execute(It.IsAny<GetAllCriteria>())).Returns(testData.ToList());
        }

        private void MockGetRequestByIdQuery(Request request)
        {
            _getRequestByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(request);
        }

        private void MockAuthServiceThrowsException(AuthOperation operation)
        {
            _authServiceMock.Setup(tr => tr.CheckAccess(operation)).Throws<AuthorizationException>();
        }

        private void MockRequestServiceThrowsInvalidKeyValueException()
        {
            _requestRepositoryMock.Setup(p => p.Remove(It.IsAny<int>())).Throws<InvalidKeyValueException>();
        }

        private void VerifyCheckAccess(AuthOperation operation, Times times)
        {
            _authServiceMock.Verify(tr => tr.CheckAccess(operation), times);
        }

        private void VerifyEditUser(User user, Times times)
        {
            _userRepositoryMock.Verify(tr => tr.Update(It.Is<User>(t => UsersAreEqual(t, user))), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }

        private bool UsersAreEqual(User x, User y)
        {
            return new UserComparer().Compare(x, y) == 0;
        }

        private void VerifyDeleteRequest(int requestId, Times times)
        {
            _requestRepositoryMock.Verify(tr => tr.Remove(It.Is<int>(id => id == requestId)), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }

        private void VerifyDeleteRequest(int requestId, Times repositoryTimes, Times unitOfWorkTimes)
        {
            _requestRepositoryMock.Verify(tr => tr.Remove(It.Is<int>(id => id == requestId)), repositoryTimes);
            _unitOfWorkMock.Verify(uow => uow.Commit(), unitOfWorkTimes);
        }

        private void MockGetUserByIdQuery(int userId, User user)
        {
            _userServiceMock.Setup(tr => tr.GetUser(userId)).Returns(user);
        }

        private void VerifyCreateRequest(Request request, Times times, string message)
        {
            _requestRepositoryMock.Verify(pr => pr.Add(It.Is<Request>(p => RequestAreEqual(p, request))), times, message);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }

        private bool RequestAreEqual(Request x, Request y)
        {
            return new RequestComparer().Compare(x, y) == 0;
        }

        #endregion
    }
}
