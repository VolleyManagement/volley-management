using System.Collections;

namespace VolleyManagement.UnitTests.Services.UsersService
{
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
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.User;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.RolesAggregate;
    using VolleyManagement.Domain.UsersAggregate;
    using VolleyManagement.Services.Authorization;
    using VolleyManagement.UnitTests.Services.PlayerService;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class UserServiceTests : BaseTest
    {
        private const int EXISTING_ID = 1;

        private Mock<IAuthorizationService> _authServiceMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ICacheProvider> _cacheProviderMock;
        private Mock<IQuery<ICollection<User>, GetAllCriteria>> _getAllQueryMock;
        private Mock<IQuery<Player, FindByIdCriteria>> _getPlayerByIdQueryMock;
        private Mock<IQuery<User, FindByIdCriteria>> _getByIdQueryMock;
        private Mock<IQuery<ICollection<User>, UniqueUserCriteria>> _getAdminsListQueryMock;
        private Mock<ICurrentUserService> _currentUserServiceMock;

        private UserServiceTestFixture _testFixture = new UserServiceTestFixture();

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _authServiceMock = new Mock<IAuthorizationService>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _cacheProviderMock = new Mock<ICacheProvider>();
            _getAllQueryMock = new Mock<IQuery<ICollection<User>, GetAllCriteria>>();
            _getPlayerByIdQueryMock = new Mock<IQuery<Player, FindByIdCriteria>>();
            _getByIdQueryMock = new Mock<IQuery<User, FindByIdCriteria>>();
            _getAdminsListQueryMock = new Mock<IQuery<ICollection<User>, UniqueUserCriteria>>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
        }

        [TestMethod]
        public void GetAll_UsersExist_UsersReturned()
        {
            // Arrange
            var testData = _testFixture.TestUsers().Build();
            MockGetAllUsersQuery(testData);

            var expected = new UserServiceTestFixture()
                                            .TestUsers()
                                            .Build()
                                            .ToList();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetAllUsers();

            // Assert
            CollectionAssert.AreEqual(expected, actual as ICollection, new UserComparer());
        }

        [TestMethod]
        public void GetAll_NoViewRights_AuthorizationExceptionThrown()
        {
            // Arrange
            MockAuthServiceThrowsException(AuthOperations.AllUsers.ViewList);

            var sut = BuildSUT();

            // Act => Assert
            Assert.Throws<AuthorizationException>(() => sut.GetAllUsers(), "Requested operation is not allowed");
        }

        [TestMethod]
        public void GetAllActiveUsers_NoViewRights_AuthorizationExceptionThrown()
        {
            // Arrange
            var testData = _testFixture.TestUsers().Build();
            MockAuthServiceThrowsException(AuthOperations.AllUsers.ViewActiveList);

            var sut = BuildSUT();

            // Act => Assert
            Assert.Throws<AuthorizationException>(() => sut.GetAllActiveUsers(), "Requested operation is not allowed");
        }

        [TestMethod]
        public void GetById_UserExists_UserReturned()
        {
            // Arrange
            var expected = new UserBuilder().WithId(EXISTING_ID).Build();
            MockGetUserByIdQuery(expected);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetUser(EXISTING_ID);

            // Assert
            TestHelper.AreEqual<User>(expected, actual, new UserComparer());
        }

        [TestMethod]
        public void GetUserDetails_NoViewRights_AuthorizationExceptionThrown()
        {
            // Arrange
            MockAuthServiceThrowsException(AuthOperations.AllUsers.ViewDetails);

            var sut = BuildSUT();

            // Act => Assert
            Assert.Throws<AuthorizationException>(() => sut.GetUserDetails(EXISTING_ID), "Requested operation is not allowed");
        }

        [TestMethod]
        public void GetUserDetails_UserExists_UserReturned()
        {
            // Arrange
            var player = new PlayerBuilder().WithId(EXISTING_ID).Build();
            var expected = new UserBuilder().WithId(EXISTING_ID).WithPlayer(player).Build();
            MockGetUserByIdQuery(expected);
            MockGetPlayerByIdQuery(player);
            var sut = BuildSUT();

            // Act
            var actual = sut.GetUserDetails(EXISTING_ID);

            // Assert
            TestHelper.AreEqual<User>(expected, actual, new UserComparer());
        }

        [TestMethod]
        public void GetAdminsList_UsersExist_UsersReturned()
        {
            // Arrange
            var testData = _testFixture.TestUsers().Build();
            MockGetAdminsListQuery(testData);

            var expected = new UserServiceTestFixture()
                                            .TestUsers()
                                            .Build()
                                            .ToList();
            var sut = BuildSUT();

            // Act
            var actual = sut.GetAdminsList();

            // Assert
            CollectionAssert.AreEqual(expected, actual as ICollection, new UserComparer());
        }

        private UserService BuildSUT()
        {
            return new UserService(
                _authServiceMock.Object,
                _getByIdQueryMock.Object,
                _getAllQueryMock.Object,
                _getPlayerByIdQueryMock.Object,
                _cacheProviderMock.Object,
                _getAdminsListQueryMock.Object,
                _userRepositoryMock.Object,
                _currentUserServiceMock.Object);
        }

        private void MockAuthServiceThrowsException(AuthOperation operation)
        {
            _authServiceMock.Setup(tr => tr.CheckAccess(operation)).Throws<AuthorizationException>();
        }

        private void MockGetAllUsersQuery(IEnumerable<User> testData)
        {
            _getAllQueryMock.Setup(tr => tr.Execute(It.IsAny<GetAllCriteria>())).Returns(testData.ToList());
        }

        private void MockGetUserByIdQuery(User testData)
        {
            _getByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(testData);
        }

        private void MockGetPlayerByIdQuery(Player testData)
        {
            _getPlayerByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(testData);
        }

        private void MockGetAdminsListQuery(IEnumerable<User> testData)
        {
            _getAdminsListQueryMock.Setup(tr => tr.Execute(It.IsAny<UniqueUserCriteria>())).Returns(testData.ToList());
        }
    }
}
