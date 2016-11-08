namespace VolleyManagement.UnitTests.Services.RequestsService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Queries.Common;
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
    public class RequestServiceTests
    {
        #region Fields and constants

        private const int EXISTING_ID = 1;

        private const int INVALID_REQUEST_ID = -1;

        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private readonly RequestServiceTestFixture _testFixture = new RequestServiceTestFixture();
        private readonly Mock<IRequestRepository> _requestRepositoryMock = new Mock<IRequestRepository>();
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private readonly Mock<IAuthorizationService> _authServiceMock = new Mock<IAuthorizationService>();
        private readonly Mock<IQuery<Request, FindByIdCriteria>> _getRequestByIdQueryMock =
            new Mock<IQuery<Request, FindByIdCriteria>>();

        private readonly Mock<IQuery<User, FindByIdCriteria>> _getUserByIdQueryMock =
            new Mock<IQuery<User, FindByIdCriteria>>();

        private readonly Mock<IQuery<List<Request>, GetAllCriteria>> _getAllRequestsQueryMock =
          new Mock<IQuery<List<Request>, GetAllCriteria>>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        private IKernel _kernel;

        #endregion

        #region Constuctor

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IUserRepository>().ToConstant(_userRepositoryMock.Object);
            _kernel.Bind<IUserService>().ToConstant(_userServiceMock.Object);
            _kernel.Bind<IRequestRepository>().ToConstant(_requestRepositoryMock.Object);
            _kernel.Bind<IQuery<Request, FindByIdCriteria>>().ToConstant(_getRequestByIdQueryMock.Object);
            _kernel.Bind<IQuery<List<Request>, GetAllCriteria>>().ToConstant(_getAllRequestsQueryMock.Object);
            _kernel.Bind<IAuthorizationService>().ToConstant(_authServiceMock.Object);
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
            var sut = _kernel.Get<RequestService>();
            var actual = sut.Get();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new RequestComparer());
        }

        [TestMethod]
        public void GetAll_NoViewListRights_AuthorizationExceptionThrows()
        {
            // Arrange
            Exception exception = null;
            MockAuthServiceThrowsException(AuthOperations.Requests.ViewList);

            var sut = _kernel.Get<RequestService>();

            // Act
            try
            {
                sut.Get();
            }
            catch (AuthorizationException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, "Requested operation is not allowed");
        }

        [TestMethod]
        public void GetById_RequestExists_RequestReturned()
        {
            // Arrange
            var expected = new RequestBuilder().WithId(EXISTING_ID).Build();
            MockGetRequestByIdQuery(expected);

            var sut = _kernel.Get<RequestService>();

            // Act
            var actual = sut.Get(EXISTING_ID);

            // Assert
            TestHelper.AreEqual<Request>(expected, actual, new RequestComparer());
        }

        [TestMethod]
        public void Approve_NoApproveRights_DbNotChanged()
        {
            // Arrange
            Exception exception = null;
            var user = new UserBuilder().WithId(EXISTING_ID).Build();
            MockAuthServiceThrowsException(AuthOperations.Requests.Approve);

            var sut = _kernel.Get<RequestService>();

            // Act
            try
            {
                sut.Approve(EXISTING_ID);
            }
            catch (AuthorizationException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyEditUser(user, Times.Never());
            VerifyCheckAccess(AuthOperations.Requests.Approve, Times.Once());
        }

        [TestMethod]
        public void Approve_RequestDoesNotExist_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;
            var sut = _kernel.Get<RequestService>();

            // Act
            try
            {
                sut.Approve(INVALID_REQUEST_ID);
            }
            catch (MissingEntityException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, "A request with specified identifier was not found");
        }

        [TestMethod]
        public void Approve_RequestExists_UserUpdated()
        {
            // Arrange
            var expected = new RequestBuilder().WithId(EXISTING_ID).Build();
            MockGetRequestByIdQuery(expected);
            var user = new UserBuilder().WithId(EXISTING_ID).Build();
            MockGetUserByIdQuery(EXISTING_ID, user);
            var sut = _kernel.Get<RequestService>();

            // Act
             sut.Approve(EXISTING_ID);

            // Assert
            VerifyEditUser(user, Times.Once());
        }

        [TestMethod]
        public void Approve_NoApproveRights_AuthorizationExceptionThrows()
        {
            // Arrange
            Exception exception = null;
            var user = new UserBuilder().WithId(EXISTING_ID).Build();
            MockAuthServiceThrowsException(AuthOperations.Requests.Approve);

            var sut = _kernel.Get<RequestService>();

            // Act
            try
            {
                sut.Approve(EXISTING_ID);
            }
            catch (AuthorizationException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, "Requested operation is not allowed");
        }

        [TestMethod]
        public void Decline_NoDeclineRights_AuthorizationExceptionThrows()
        {
            // Arrange
            Exception exception = null;
            var user = new UserBuilder().WithId(EXISTING_ID).Build();
            MockAuthServiceThrowsException(AuthOperations.Requests.Decline);

            var sut = _kernel.Get<RequestService>();

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
            VerifyExceptionThrown(exception, "Requested operation is not allowed");
        }

        [TestMethod]
        public void Decline_NoDeclineRights_DbNotChanged()
        {
            // Arrange
            Exception exception = null;
            MockAuthServiceThrowsException(AuthOperations.Requests.Decline);

            var sut = _kernel.Get<RequestService>();

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
            var sut = _kernel.Get<RequestService>();

            // Act
            sut.Decline(EXISTING_ID);

            // Assert
            VerifyDeleteRequest(EXISTING_ID, Times.Once());
        }
        #endregion

        #region Private
        private void MockGetAllRequestsQuery(IEnumerable<Request> testData)
        {
            _getAllRequestsQueryMock.Setup(tr => tr.Execute(It.IsAny<GetAllCriteria>())).Returns(testData.ToList());
        }

        private void MockGetRequestByIdQuery(Request request)
        {
            _getRequestByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(request);
        }

        private void VerifyExceptionThrown(Exception exception, string expectedMessage)
        {
            Assert.IsNotNull(exception, "There is no exception thrown");
            Assert.IsTrue(exception.Message.Equals(expectedMessage), "Expected and actual exceptions messages aren't equal");
        }

        private void MockAuthServiceThrowsException(AuthOperation operation)
        {
            _authServiceMock.Setup(tr => tr.CheckAccess(operation)).Throws<AuthorizationException>();
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

        private void MockGetUserByIdQuery(int userId, User user)
        {
            _userServiceMock.Setup(tr => tr.GetUser(userId)).Returns(user);
        }

        #endregion
    }
}
